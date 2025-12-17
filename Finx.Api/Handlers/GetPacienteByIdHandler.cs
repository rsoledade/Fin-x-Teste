using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Finx.Domain;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers
{
    public class GetPacienteByIdHandler : IRequestHandler<GetPacienteByIdQuery, PacienteDto?>
    {
        private readonly IPacienteRepository _repo;

        public GetPacienteByIdHandler(IPacienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<PacienteDto?> Handle(GetPacienteByIdQuery request, CancellationToken cancellationToken)
        {
            var paciente = await _repo.GetByIdAsync(request.Id);
            if (paciente == null) return null;

            return new PacienteDto
            {
                Id = paciente.Id,
                Nome = paciente.Nome,
                Cpf = paciente.Cpf,
                DataNascimento = paciente.DataNascimento,
                DataCadastro = paciente.DataCadastro,
                Contato = paciente.Contato
            };
        }
    }
}