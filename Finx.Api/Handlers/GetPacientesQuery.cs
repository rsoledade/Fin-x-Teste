using MediatR;
using System.Collections.Generic;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers
{
    public record GetPacientesQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<PacienteDto>>;
}