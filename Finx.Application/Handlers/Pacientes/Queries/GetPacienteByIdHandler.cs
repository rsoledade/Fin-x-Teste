using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Pacientes.Queries;

public sealed class GetPacienteByIdHandler : IRequestHandler<GetPacienteByIdQuery, Paciente?>
{
    private readonly IPacienteRepository _repo;

    public GetPacienteByIdHandler(IPacienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<Paciente?> Handle(GetPacienteByIdQuery request, CancellationToken cancellationToken)
        => await _repo.GetByIdAsync(request.Id);
}
