using MediatR;

namespace Finx.Api.Handlers.Pacientes.Commands
{
    public record UpdatePacienteCommand(Guid Id, string Nome, string Cpf, DateTime? DataNascimento, string? Contato) : IRequest<bool>;
}
