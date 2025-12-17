using Finx.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Finx.Api.Services;
using FluentValidation;
using Finx.Api.Behaviors;
using Finx.Api.Middleware;
using Finx.Domain;
using Finx.Infrastructure.Repositories;
using Finx.Api.Validators;
using Finx.Api.DTOs;
using Finx.Api.Handlers;
using Finx.Integrations.Contracts;
using Finx.Integrations.Adapters;
using Serilog;
using System.Threading;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.StackExchangeRedis;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure DbContext: prefer SQL Server if connection provided, otherwise InMemory for dev
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrWhiteSpace(defaultConn))
{
    builder.Services.AddDbContext<FinxDbContext>(options =>
        options.UseSqlServer(defaultConn));
}
else
{
    builder.Services.AddDbContext<FinxDbContext>(options =>
        options.UseInMemoryDatabase("FinxDb"));
}

// Redis cache registration if configured
var redisConfig = builder.Configuration["Redis:Configuration"];
if (!string.IsNullOrWhiteSpace(redisConfig))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisConfig;
    });
}

// Register MediatR (scan Finx.Api assembly for handlers)
builder.Services.AddMediatR(typeof(Program).Assembly);

// Register repository implementations
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();

builder.Services.AddControllers();

// Health checks - use a custom health check for the Db
builder.Services.AddHealthChecks()
    .AddCheck<Finx.Api.Health.FinxDbHealthCheck>("FinxDb", tags: new[] { "ready" });

// Register FluentValidation validators explicitly
builder.Services.AddTransient<CreatePacienteCommandValidator>();
builder.Services.AddTransient<CreatePacienteDtoValidator>();
builder.Services.AddTransient<LoginRequestValidator>();
builder.Services.AddTransient<HistoricoDtoValidator>();

// Register pipeline behavior for validation
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Register Exame clients and FileStorage
builder.Services.AddHttpClient<ExameHttpClient>(client =>
{
    client.BaseAddress = new System.Uri(builder.Configuration["ExameApi:BaseUrl"] ?? "https://api.exames.example/");
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddSingleton<MockExameClient>();
builder.Services.AddScoped<IExameClient, ExameClientWithFallback>();
// Local file storage for development
builder.Services.AddSingleton<IFileStorage>(new LocalFileStorage(builder.Configuration["FileStorage:BasePath"] ?? "./filestorage"));

// JWT configuration (use environment variable or user-secrets in real apps)
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "very_secret_key_for_dev_only";
var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddSingleton(new JwtTokenService(jwtSecret));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Apply migrations or ensure DB created — validate and only apply if pending
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<FinxDbContext>();

    try
    {
        // Wait for DB to be available (only for non-inmemory providers)
        if (!db.Database.IsInMemory())
        {
            var tries = 0;
            var maxTries = 10;
            var delay = TimeSpan.FromSeconds(5);
            while (tries < maxTries)
            {
                try
                {
                    if (db.Database.CanConnect()) break;
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Database not ready yet");
                }
                tries++;
                Thread.Sleep(delay);
            }

            if (!db.Database.CanConnect())
            {
                logger.LogWarning("Database unavailable after retries; skipping migrations");
            }
            else
            {
                var pending = db.Database.GetPendingMigrations();
                if (pending != null && pending.Any())
                {
                    logger.LogInformation("Applying {Count} pending migrations", pending.Count());
                    db.Database.Migrate();
                    logger.LogInformation("Migrations applied");
                }
                else
                {
                    logger.LogInformation("No pending migrations to apply");
                }
            }
        }
        else
        {
            // InMemory ensure created
            db.Database.EnsureCreated();
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while applying migrations");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Add validation exception middleware early in pipeline
app.UseMiddleware<ValidationExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Map health checks
app.MapHealthChecks("/health/ready", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { Predicate = hc => hc.Tags.Contains("ready") });
app.MapHealthChecks("/health/live", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions { Predicate = _ => false });

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
