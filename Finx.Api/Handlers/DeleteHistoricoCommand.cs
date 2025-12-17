using System;
using MediatR;

namespace Finx.Api.Handlers
{
    public record DeleteHistoricoCommand(Guid Id) : IRequest<bool>;
}
