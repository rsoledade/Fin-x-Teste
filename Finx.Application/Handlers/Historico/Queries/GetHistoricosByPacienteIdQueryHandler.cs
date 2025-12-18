using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Historico.Queries;

public sealed class GetHistoricosByPacienteIdQueryHandler : IRequestHandler<GetHistoricosByPacienteIdQuery, IEnumerable<HistoricoMedico>>
{
    private readonly IHistoricoMedicoRepository _repo;

    public GetHistoricosByPacienteIdQueryHandler(IHistoricoMedicoRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<HistoricoMedico>> Handle(GetHistoricosByPacienteIdQuery request, CancellationToken cancellationToken)
        => await _repo.ListByPacienteIdAsync(request.PacienteId);
}
