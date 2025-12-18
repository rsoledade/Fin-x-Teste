using MediatR;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Pacientes.Commands;

public sealed class UpdatePacienteCommandHandler : IRequestHandler<UpdatePacienteCommand>
{
    private readonly IPacienteRepository _repo;
    private readonly ILogger<UpdatePacienteCommandHandler> _logger;

    public UpdatePacienteCommandHandler(IPacienteRepository repo, ILogger<UpdatePacienteCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Unit> Handle(UpdatePacienteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Updating paciente. Id={PacienteId}", request.Id);

            var paciente = await _repo.GetByIdAsync(request.Id);
            if (paciente is null)
            {
                _logger.LogWarning("Paciente not found for update. Id={PacienteId}", request.Id);
                return Unit.Value;
            }

            paciente.Nome = request.Nome;
            paciente.DataNascimento = request.DataNascimento;
            paciente.Contato = request.Contato;

            await _repo.UpdateAsync(paciente);

            _logger.LogInformation("Paciente updated. Id={PacienteId}", request.Id);
            return Unit.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating paciente. Id={PacienteId}", request.Id);
            throw;
        }
    }
}
