using MediatR;
using Finx.Api.Behaviors;
using Finx.Api.Validators;

namespace Finx.Api.Configuration
{
    public static class ValidationConfiguration
    {
        public static IServiceCollection AddValidationConfiguration(this IServiceCollection services)
        {
            // Register FluentValidation validators explicitly
            services.AddTransient<LoginRequestValidator>();
            services.AddTransient<HistoricoDtoValidator>();
            services.AddTransient<CreatePacienteDtoValidator>();
            services.AddTransient<UpdatePacienteCommandValidator>();
            services.AddTransient<CreatePacienteCommandValidator>();

            // Register pipeline behavior for validation
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
