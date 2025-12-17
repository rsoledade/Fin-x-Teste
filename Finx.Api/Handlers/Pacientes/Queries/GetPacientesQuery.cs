using MediatR;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers.Pacientes.Queries
{
    public record GetPacientesQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<PacienteDto>>;
}
