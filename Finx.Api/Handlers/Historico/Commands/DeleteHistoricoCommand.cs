using System;
using MediatR;

namespace Finx.Api.Handlers.Historico.Commands
{
    public record DeleteHistoricoCommand(Guid Id) : IRequest<bool>;
}
