using Finx.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddLoggingConfiguration();

// Swagger UI (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddCacheConfiguration(builder.Configuration);
builder.Services.AddMediatRConfiguration();
builder.Services.AddRepositories();
builder.Services.AddIntegrations(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthCheckConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCustomMiddleware();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapHealthCheckEndpoints();

// Apply database migrations after DI is built and before serving requests
await app.UseDatabaseMigration();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }


