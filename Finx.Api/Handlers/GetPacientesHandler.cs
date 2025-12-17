using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Finx.Domain;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers
{
    public class GetPacientesHandler : IRequestHandler<GetPacientesQuery, IEnumerable<PacienteDto>>
    {
        private readonly IPacienteRepository _repo;

        public GetPacientesHandler(IPacienteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PacienteDto>> Handle(GetPacientesQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.ListAsync();
            return list.Select(p => new PacienteDto
            {
                Id = p.Id,
                Nome = p.Nome,
                Cpf = p.Cpf,
                DataNascimento = p.DataNascimento,
                DataCadastro = p.DataCadastro,
                Contato = p.Contato
            });
        }
    }
}