using MediatR;
using Finx.Domain.Entities;

namespace Finx.Application.Handlers.Historico.Queries;

public record GetHistoricosByPacienteIdQuery(Guid PacienteId) : IRequest<IEnumerable<HistoricoMedico>>;
