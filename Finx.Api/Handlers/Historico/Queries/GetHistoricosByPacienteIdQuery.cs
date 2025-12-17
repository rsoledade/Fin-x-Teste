using MediatR;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers.Historico.Queries
{
    public record GetHistoricosByPacienteIdQuery(Guid PacienteId) : IRequest<IEnumerable<HistoricoDto>>;
}
