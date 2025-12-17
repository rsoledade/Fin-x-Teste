using MediatR;

namespace Finx.Api.Handlers.Pacientes.Commands
{
    public record DeletePacienteCommand(Guid Id) : IRequest<bool>;
}
