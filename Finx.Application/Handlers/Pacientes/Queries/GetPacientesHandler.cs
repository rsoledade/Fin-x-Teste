using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Pacientes.Queries;

public sealed class GetPacientesHandler : IRequestHandler<GetPacientesQuery, IEnumerable<Paciente>>
{
    private readonly IPacienteRepository _repo;

    public GetPacientesHandler(IPacienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Paciente>> Handle(GetPacientesQuery request, CancellationToken cancellationToken)
        => await _repo.ListAsync(request.Page, request.PageSize);
}
