using System;
using System.Collections.Generic;
using MediatR;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers
{
    public record GetHistoricosByPacienteIdQuery(Guid PacienteId) : IRequest<IEnumerable<HistoricoDto>>;
}
