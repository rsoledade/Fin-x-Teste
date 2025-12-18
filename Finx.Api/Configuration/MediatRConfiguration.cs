using MediatR;

namespace Finx.Api.Configuration
{
    public static class MediatRConfiguration
    {
        public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
        {
            services.AddMediatR(typeof(Program).Assembly, typeof(Finx.Application.Handlers.Pacientes.Commands.CreatePacienteCommand).Assembly);
            return services;
        }
    }
}
