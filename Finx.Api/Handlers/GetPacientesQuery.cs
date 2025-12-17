using MediatR;
using System.Collections.Generic;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers
{
    public record GetPacientesQuery() : IRequest<IEnumerable<PacienteDto>>;
}