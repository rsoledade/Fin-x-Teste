using FluentValidation;
using Finx.Application.Handlers.Pacientes.Commands;

namespace Finx.Application.Validators;

public sealed class CreatePacienteCommandValidator : AbstractValidator<CreatePacienteCommand>
{
    public CreatePacienteCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Cpf)
            .NotEmpty().WithMessage("CPF é obrigatório")
            .Must(IsValidCpf).WithMessage("CPF inválido");
        RuleFor(x => x.DataNascimento)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Data de nascimento inválida");
        RuleFor(x => x.Contato).MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Contato));
    }

    private static bool IsValidCpf(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;

        var digits = new string(cpf.Where(char.IsDigit).ToArray());
        if (digits.Length != 11) return false;

        var invalids = new[]
        {
            "00000000000","11111111111","22222222222","33333333333","44444444444",
            "55555555555","66666666666","77777777777","88888888888","99999999999"
        };

        if (invalids.Contains(digits)) return false;

        var numbers = digits.Select(c => c - '0').ToArray();
        var sum = 0;

        for (var i = 0; i < 9; i++) sum += numbers[i] * (10 - i);
        var remainder = sum % 11;
        var firstVerifier = remainder < 2 ? 0 : 11 - remainder;
        if (numbers[9] != firstVerifier) return false;

        sum = 0;
        for (var i = 0; i < 10; i++) sum += numbers[i] * (11 - i);
        remainder = sum % 11;
        var secondVerifier = remainder < 2 ? 0 : 11 - remainder;
        if (numbers[10] != secondVerifier) return false;

        return true;
    }
}
