using Finx.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register DbContext (use InMemory for now to simplify local dev)
builder.Services.AddDbContext<FinxDbContext>(options =>
    options.UseInMemoryDatabase("FinxDb"));

// Register MediatR (scan Finx.Api assembly for handlers)
builder.Services.AddMediatR(typeof(Program).Assembly);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
