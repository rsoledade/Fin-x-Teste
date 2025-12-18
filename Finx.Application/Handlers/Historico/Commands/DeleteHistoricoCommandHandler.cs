using MediatR;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Historico.Commands;

public sealed class DeleteHistoricoCommandHandler : IRequestHandler<DeleteHistoricoCommand>
{
    private readonly IHistoricoMedicoRepository _repo;

    public DeleteHistoricoCommandHandler(IHistoricoMedicoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(DeleteHistoricoCommand request, CancellationToken cancellationToken)
    {
        var historico = await _repo.GetByIdAsync(request.Id);
        if (historico is null) return Unit.Value;

        if (historico.PacienteId != request.PacienteId) return Unit.Value;

        await _repo.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
