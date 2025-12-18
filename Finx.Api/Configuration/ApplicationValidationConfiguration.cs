using MediatR;
using FluentValidation;
using Finx.Application.Behaviors;

namespace Finx.Api.Configuration
{
    public static class ApplicationValidationConfiguration
    {
        public static IServiceCollection AddApplicationValidationConfiguration(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(Finx.Application.Validators.CreatePacienteCommandValidator).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
