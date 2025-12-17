namespace Finx.Api.Configuration
{
    public static class HealthCheckConfiguration
    {
        public static IServiceCollection AddHealthCheckConfiguration(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<Health.FinxDbHealthCheck>("FinxDb", tags: new[] { "ready" });

            return services;
        }

        public static WebApplication MapHealthCheckEndpoints(this WebApplication app)
        {
            app.MapHealthChecks("/health/ready", 
                new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions 
                { 
                    Predicate = hc => hc.Tags.Contains("ready") 
                });
                
            app.MapHealthChecks("/health/live", 
                new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions 
                { 
                    Predicate = _ => false 
                });

            return app;
        }
    }
}
