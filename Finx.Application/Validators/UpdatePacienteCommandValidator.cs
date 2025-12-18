using FluentValidation;
using Finx.Application.Handlers.Pacientes.Commands;

namespace Finx.Application.Validators;

public sealed class UpdatePacienteCommandValidator : AbstractValidator<UpdatePacienteCommand>
{
    public UpdatePacienteCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DataNascimento)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Data de nascimento inválida");
        RuleFor(x => x.Contato).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Contato));
    }
}
