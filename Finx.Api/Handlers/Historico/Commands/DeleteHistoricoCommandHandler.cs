using MediatR;
using Finx.Domain.Repositories;

namespace Finx.Api.Handlers.Historico.Commands
{
    public class DeleteHistoricoCommandHandler : IRequestHandler<DeleteHistoricoCommand, bool>
    {
        private readonly IHistoricoMedicoRepository _repo;

        public DeleteHistoricoCommandHandler(IHistoricoMedicoRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteHistoricoCommand request, CancellationToken cancellationToken)
        {
            var historico = await _repo.GetByIdAsync(request.Id);
            if (historico == null)
                return false;

            await _repo.DeleteAsync(request.Id);
            return true;
        }
    }
}
