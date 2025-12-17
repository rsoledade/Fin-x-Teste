using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;

namespace Finx.Api.Handlers.Pacientes.Commands
{
    public class CreatePacienteCommandHandler : IRequestHandler<CreatePacienteCommand, Guid>
    {
        private readonly IPacienteRepository _repo;

        public CreatePacienteCommandHandler(IPacienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<Guid> Handle(CreatePacienteCommand request, CancellationToken cancellationToken)
        {
            var paciente = new Paciente
            {
                Nome = request.Nome,
                Cpf = request.Cpf,
                DataNascimento = request.DataNascimento,
                DataCadastro = DateTime.UtcNow,
                Contato = request.Contato
            };

            var id = await _repo.AddAsync(paciente);
            return id;
        }
    }
}
