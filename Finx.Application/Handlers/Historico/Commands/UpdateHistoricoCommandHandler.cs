using MediatR;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Historico.Commands;

public sealed class UpdateHistoricoCommandHandler : IRequestHandler<UpdateHistoricoCommand>
{
    private readonly IHistoricoMedicoRepository _repo;

    public UpdateHistoricoCommandHandler(IHistoricoMedicoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(UpdateHistoricoCommand request, CancellationToken cancellationToken)
    {
        var historico = await _repo.GetByIdAsync(request.Id);
        if (historico is null) return Unit.Value;

        historico.PacienteId = request.PacienteId;
        historico.Diagnostico = request.Diagnostico;
        historico.Exame = request.Exame;
        historico.Prescricao = request.Prescricao;
        historico.Data = request.Data;

        await _repo.UpdateAsync(historico);
        return Unit.Value;
    }
}
