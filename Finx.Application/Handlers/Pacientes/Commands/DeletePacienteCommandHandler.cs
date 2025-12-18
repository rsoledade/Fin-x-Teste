using MediatR;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Pacientes.Commands;

public sealed class DeletePacienteCommandHandler : IRequestHandler<DeletePacienteCommand>
{
    private readonly IPacienteRepository _repo;
    private readonly ILogger<DeletePacienteCommandHandler> _logger;

    public DeletePacienteCommandHandler(IPacienteRepository repo, ILogger<DeletePacienteCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeletePacienteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Deleting paciente. Id={PacienteId}", request.Id);

            await _repo.DeleteAsync(request.Id);

            _logger.LogInformation("Paciente deleted. Id={PacienteId}", request.Id);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting paciente. Id={PacienteId}", request.Id);
            throw;
        }
    }
}
