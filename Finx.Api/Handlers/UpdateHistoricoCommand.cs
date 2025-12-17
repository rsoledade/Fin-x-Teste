using System;
using MediatR;

namespace Finx.Api.Handlers
{
    public record UpdateHistoricoCommand(Guid Id, string Diagnostico, string Exame, string Prescricao, DateTime Data) : IRequest<bool>;
}
