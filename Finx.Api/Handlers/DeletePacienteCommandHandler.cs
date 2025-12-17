using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Finx.Domain;

namespace Finx.Api.Handlers
{
    public class DeletePacienteCommandHandler : IRequestHandler<DeletePacienteCommand, bool>
    {
        private readonly IPacienteRepository _repo;

        public DeletePacienteCommandHandler(IPacienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeletePacienteCommand request, CancellationToken cancellationToken)
        {
            var paciente = await _repo.GetByIdAsync(request.Id);
            if (paciente == null)
                return false;

            await _repo.DeleteAsync(request.Id);
            return true;
        }
    }
}
