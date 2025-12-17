using Finx.Integrations.Adapters;
using Finx.Integrations.Contracts;

namespace Finx.Api.Configuration
{
    public static class IntegrationConfiguration
    {
        public static IServiceCollection AddIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Exame clients
            services.AddHttpClient<ExameHttpClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["ExameApi:BaseUrl"] ?? "https://api.exames.example/");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            services.AddSingleton<MockExameClient>();
            services.AddScoped<IExameClient, ExameClientWithFallback>();

            // Register FileStorage
            var fileStoragePath = configuration["FileStorage:BasePath"] ?? "./filestorage";
            services.AddSingleton<IFileStorage>(new LocalFileStorage(fileStoragePath));

            return services;
        }
    }
}
