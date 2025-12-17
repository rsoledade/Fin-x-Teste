using System;
using MediatR;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers
{
    public record GetPacienteByIdQuery(Guid Id) : IRequest<PacienteDto>;
}