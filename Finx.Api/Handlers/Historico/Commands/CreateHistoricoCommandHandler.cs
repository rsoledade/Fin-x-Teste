using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;

namespace Finx.Api.Handlers.Historico.Commands
{
    public class CreateHistoricoCommandHandler : IRequestHandler<CreateHistoricoCommand, Guid>
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

            var id = await _repo.AddAsync(historico);
            return id;
        }
    }
}
