using FluentValidation;
using Finx.Api.DTOs;

namespace Finx.Api.Validators
{
    public class CreatePacienteDtoValidator : AbstractValidator<CreatePacienteDto>
    {
        public CreatePacienteDtoValidator()
        {
            RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Cpf).NotEmpty().Length(11).WithMessage("CPF deve conter 11 dígitos (apenas números)");
            RuleFor(x => x.DataNascimento).LessThanOrEqualTo(System.DateTime.UtcNow).WithMessage("Data de nascimento inválida");
            RuleFor(x => x.Contato).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Contato));
        }
    }
}
