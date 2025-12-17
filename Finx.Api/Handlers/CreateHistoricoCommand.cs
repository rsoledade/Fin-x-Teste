using System;
using MediatR;

namespace Finx.Api.Handlers
{
    public record CreateHistoricoCommand(Guid PacienteId, string Diagnostico, string Exame, string Prescricao, DateTime Data) : IRequest<Guid>;
}
