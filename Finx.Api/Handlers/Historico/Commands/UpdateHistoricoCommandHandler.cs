using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Finx.Domain.Repositories;

namespace Finx.Api.Handlers.Historico.Commands
{
    public class UpdateHistoricoCommandHandler : IRequestHandler<UpdateHistoricoCommand, bool>
    {
        private readonly IHistoricoMedicoRepository _repo;

        public UpdateHistoricoCommandHandler(IHistoricoMedicoRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateHistoricoCommand request, CancellationToken cancellationToken)
        {
            var historico = await _repo.GetByIdAsync(request.Id);
            if (historico == null)
                return false;

            historico.Diagnostico = request.Diagnostico;
            historico.Exame = request.Exame;
            historico.Prescricao = request.Prescricao;
            historico.Data = request.Data;

            await _repo.UpdateAsync(historico);
            return true;
        }
    }
}
