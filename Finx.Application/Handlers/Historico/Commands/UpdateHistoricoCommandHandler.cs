using MediatR;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Historico.Commands;

public sealed class UpdateHistoricoCommandHandler : IRequestHandler<UpdateHistoricoCommand>
{
    private readonly IHistoricoMedicoRepository _repo;
    private readonly ILogger<UpdateHistoricoCommandHandler> _logger;

    public UpdateHistoricoCommandHandler(IHistoricoMedicoRepository repo, ILogger<UpdateHistoricoCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdateHistoricoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating historico. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);

            var historico = await _repo.GetByIdAsync(request.Id);
            if (historico is null)
            {
                _logger.LogWarning("Historico not found for update. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);
                return Unit.Value;
            }

            historico.PacienteId = request.PacienteId;
            historico.Diagnostico = request.Diagnostico;
            historico.Exame = request.Exame;
            historico.Prescricao = request.Prescricao;
            historico.Data = request.Data;

            await _repo.UpdateAsync(historico);

            _logger.LogInformation("Historico updated. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating historico. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);
            throw;
        }
    }
}
