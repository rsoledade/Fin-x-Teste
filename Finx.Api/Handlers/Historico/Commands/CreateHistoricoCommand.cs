using MediatR;

namespace Finx.Api.Handlers.Historico.Commands
{
    public record CreateHistoricoCommand(Guid PacienteId, string Diagnostico, string Exame, string Prescricao, DateTime Data) : IRequest<Guid>;
}
