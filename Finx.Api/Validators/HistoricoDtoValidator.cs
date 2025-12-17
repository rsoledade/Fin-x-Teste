using Finx.Api.DTOs;
using FluentValidation;

namespace Finx.Api.Validators
{
    public class HistoricoDtoValidator : AbstractValidator<HistoricoDto>
    {
        public HistoricoDtoValidator()
        {
            RuleFor(x => x.Diagnostico).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.Exame).MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Exame));
            RuleFor(x => x.Prescricao).MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Prescricao));
            RuleFor(x => x.Data).LessThanOrEqualTo(System.DateTime.UtcNow);
        }
    }
}
