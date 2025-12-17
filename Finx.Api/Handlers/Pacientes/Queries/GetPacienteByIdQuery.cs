using MediatR;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers.Pacientes.Queries
{
    public record GetPacienteByIdQuery(Guid Id) : IRequest<PacienteDto?>;
}
