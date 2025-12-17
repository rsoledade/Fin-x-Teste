using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;

namespace Finx.Api.Handlers.Pacientes.Commands
{
    public class UpdatePacienteCommandHandler : IRequestHandler<UpdatePacienteCommand, bool>
    {
        private readonly IPacienteRepository _repo;

        public UpdatePacienteCommandHandler(IPacienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdatePacienteCommand request, CancellationToken cancellationToken)
        {
            var paciente = await _repo.GetByIdAsync(request.Id);
            if (paciente == null)
                return false;

            paciente.Nome = request.Nome;
            paciente.Cpf = request.Cpf;
            paciente.DataNascimento = request.DataNascimento;
            paciente.Contato = request.Contato;

            await _repo.UpdateAsync(paciente);
            return true;
        }
    }
}
