using MediatR;

namespace Finx.Api.Configuration
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            // Register MediatR (scan Finx.Api assembly for handlers)
            services.AddMediatR(typeof(Program).Assembly);

            return services;
        }
    }
}
