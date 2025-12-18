using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Pacientes.Queries;

public sealed class GetPacientesHandler : IRequestHandler<GetPacientesQuery, IEnumerable<Paciente>>
{
    private readonly IPacienteRepository _repo;
    private readonly ILogger<GetPacientesHandler> _logger;

    public GetPacientesHandler(IPacienteRepository repo, ILogger<GetPacientesHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<IEnumerable<Paciente>> Handle(GetPacientesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Listing pacientes. Page={Page} PageSize={PageSize}", request.Page, request.PageSize);
            return await _repo.ListAsync(request.Page, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error listing pacientes. Page={Page} PageSize={PageSize}", request.Page, request.PageSize);
            throw;
        }
    }
}
