using MediatR;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Pacientes.Commands;

public sealed class DeletePacienteCommandHandler : IRequestHandler<DeletePacienteCommand>
{
    private readonly IPacienteRepository _repo;

    public DeletePacienteCommandHandler(IPacienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(DeletePacienteCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
