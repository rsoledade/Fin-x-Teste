using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Historico.Commands;

public sealed class CreateHistoricoCommandHandler : IRequestHandler<CreateHistoricoCommand, Guid>
{
    private readonly IHistoricoMedicoRepository _repo;
    private readonly ILogger<CreateHistoricoCommandHandler> _logger;

    public CreateHistoricoCommandHandler(IHistoricoMedicoRepository repo, ILogger<CreateHistoricoCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateHistoricoCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating historico. PacienteId={PacienteId} Data={Data}", request.PacienteId, request.Data);

            var historico = new HistoricoMedico
            {
                PacienteId = request.PacienteId,
                Diagnostico = request.Diagnostico,
                Exame = request.Exame,
                Prescricao = request.Prescricao,
                Data = request.Data
            };

            var id = await _repo.AddAsync(historico);

            _logger.LogInformation("Historico created. Id={HistoricoId} PacienteId={PacienteId}", id, request.PacienteId);
            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating historico. PacienteId={PacienteId} Data={Data}", request.PacienteId, request.Data);
            throw;
        }
    }
}
