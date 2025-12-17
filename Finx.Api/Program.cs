using Finx.Api.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.AddLoggingConfiguration();

// Services
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddCacheConfiguration(builder.Configuration);
builder.Services.AddMediatRConfiguration();
builder.Services.AddRepositories();
builder.Services.AddValidationConfiguration();
builder.Services.AddIntegrations(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddHealthCheckConfiguration();

var app = builder.Build();

// Database migrations
await app.UseDatabaseMigration();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCustomMiddleware();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapHealthCheckEndpoints();

app.Run();

// Make Program class accessible for integration tests
public partial class Program { }


