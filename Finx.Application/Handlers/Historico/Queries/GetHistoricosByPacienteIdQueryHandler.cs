using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Historico.Queries;

public sealed class GetHistoricosByPacienteIdQueryHandler : IRequestHandler<GetHistoricosByPacienteIdQuery, IEnumerable<HistoricoMedico>>
{
    private readonly IHistoricoMedicoRepository _repo;
    private readonly ILogger<GetHistoricosByPacienteIdQueryHandler> _logger;

    public GetHistoricosByPacienteIdQueryHandler(IHistoricoMedicoRepository repo, ILogger<GetHistoricosByPacienteIdQueryHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<HistoricoMedico>> Handle(GetHistoricosByPacienteIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Listing historicos. PacienteId={PacienteId}", request.PacienteId);
            return await _repo.ListByPacienteIdAsync(request.PacienteId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing historicos. PacienteId={PacienteId}", request.PacienteId);
            throw;
        }
    }
}
