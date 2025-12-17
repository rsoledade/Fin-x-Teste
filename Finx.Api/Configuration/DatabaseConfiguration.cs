using Microsoft.EntityFrameworkCore;
using Finx.Infrastructure;

namespace Finx.Api.Configuration
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var defaultConn = configuration.GetConnectionString("DefaultConnection");
            
            if (!string.IsNullOrWhiteSpace(defaultConn))
            {
                services.AddDbContext<FinxDbContext>(options =>
                    options.UseSqlServer(defaultConn));
            }
            else
            {
                services.AddDbContext<FinxDbContext>(options =>
                    options.UseInMemoryDatabase("FinxDb"));
            }

            return services;
        }

        public static async Task<IApplicationBuilder> UseDatabaseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            var db = scope.ServiceProvider.GetRequiredService<FinxDbContext>();

            try
            {
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
                        await Task.Delay(delay);
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
                            await db.Database.MigrateAsync();
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
                    await db.Database.EnsureCreatedAsync();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while applying migrations");
            }

            return app;
        }
    }
}
