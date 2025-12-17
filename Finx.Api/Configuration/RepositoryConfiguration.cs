using Finx.Domain.Repositories;
using Finx.Infrastructure.Repositories;

namespace Finx.Api.Configuration
{
    public static class RepositoryConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IPacienteRepository, PacienteRepository>();
            services.AddScoped<IHistoricoMedicoRepository, HistoricoMedicoRepository>();

            return services;
        }
    }
}
