using MediatR;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Historico.Commands;

public sealed class DeleteHistoricoCommandHandler : IRequestHandler<DeleteHistoricoCommand>
{
    private readonly IHistoricoMedicoRepository _repo;
    private readonly ILogger<DeleteHistoricoCommandHandler> _logger;

    public DeleteHistoricoCommandHandler(IHistoricoMedicoRepository repo, ILogger<DeleteHistoricoCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteHistoricoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting historico. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);

            var historico = await _repo.GetByIdAsync(request.Id);
            if (historico is null)
            {
                _logger.LogWarning("Historico not found for delete. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);
                return Unit.Value;
            }

            if (historico.PacienteId != request.PacienteId)
            {
                _logger.LogWarning(
                    "Historico paciente mismatch. HistoricoId={HistoricoId} ExpectedPacienteId={ExpectedPacienteId} ActualPacienteId={ActualPacienteId}",
                    request.Id,
                    request.PacienteId,
                    historico.PacienteId);
                return Unit.Value;
            }

            await _repo.DeleteAsync(request.Id);

            _logger.LogInformation("Historico deleted. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting historico. Id={HistoricoId} PacienteId={PacienteId}", request.Id, request.PacienteId);
            throw;
        }
    }
}
