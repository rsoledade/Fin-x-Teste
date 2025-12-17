using MediatR;
using Finx.Api.DTOs;
using Finx.Domain.Repositories;

namespace Finx.Api.Handlers.Historico.Queries
{
    public class GetHistoricosByPacienteIdQueryHandler : IRequestHandler<GetHistoricosByPacienteIdQuery, IEnumerable<HistoricoDto>>
    {
        private readonly IHistoricoMedicoRepository _repo;

        public GetHistoricosByPacienteIdQueryHandler(IHistoricoMedicoRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<HistoricoDto>> Handle(GetHistoricosByPacienteIdQuery request, CancellationToken cancellationToken)
        {
            var historicos = await _repo.ListByPacienteIdAsync(request.PacienteId);
            return historicos.Select(h => new HistoricoDto
            {
                Id = h.Id,
                PacienteId = h.PacienteId,
                Diagnostico = h.Diagnostico,
                Exame = h.Exame,
                Prescricao = h.Prescricao,
                Data = h.Data
            });
        }
    }
}
