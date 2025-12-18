using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Pacientes.Queries;

public sealed class GetPacienteByIdHandler : IRequestHandler<GetPacienteByIdQuery, Paciente?>
{
    private readonly IPacienteRepository _repo;
    private readonly ILogger<GetPacienteByIdHandler> _logger;

    public GetPacienteByIdHandler(IPacienteRepository repo, ILogger<GetPacienteByIdHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Paciente?> Handle(GetPacienteByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Getting paciente by id. Id={PacienteId}", request.Id);

            var paciente = await _repo.GetByIdAsync(request.Id);
            if (paciente is null)
            {
                _logger.LogWarning("Paciente not found. Id={PacienteId}", request.Id);
            }

            return paciente;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paciente by id. Id={PacienteId}", request.Id);
            throw;
        }
    }
}
