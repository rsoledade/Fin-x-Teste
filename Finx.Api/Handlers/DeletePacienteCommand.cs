using System;
using MediatR;

namespace Finx.Api.Handlers
{
    public record DeletePacienteCommand(Guid Id) : IRequest<bool>;
}
