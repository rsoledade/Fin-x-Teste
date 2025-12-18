using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Historico.Commands;

public sealed class CreateHistoricoCommandHandler : IRequestHandler<CreateHistoricoCommand, Guid>
{
    private readonly IHistoricoMedicoRepository _repo;

    public CreateHistoricoCommandHandler(IHistoricoMedicoRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(CreateHistoricoCommand request, CancellationToken cancellationToken)
    {
        var historico = new HistoricoMedico
        {
            PacienteId = request.PacienteId,
            Diagnostico = request.Diagnostico,
            Exame = request.Exame,
            Prescricao = request.Prescricao,
            Data = request.Data
        };

        return await _repo.AddAsync(historico);        
    }
}
