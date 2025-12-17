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

var builder = WebApplication.CreateBuilder(args);

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

// Register MediatR (scan Finx.Api assembly for handlers)
builder.Services.AddMediatR(typeof(Program).Assembly);

// Register repository implementations
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();

builder.Services.AddControllers();

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

// Apply migrations or ensure DB created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinxDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        // Log exception if logging configured; swallow to allow app to start in dev
        Console.WriteLine($"Database initialization error: {ex.Message}");
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
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
