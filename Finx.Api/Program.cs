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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register DbContext (use InMemory for now to simplify local dev)
builder.Services.AddDbContext<FinxDbContext>(options =>
    options.UseInMemoryDatabase("FinxDb"));

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
