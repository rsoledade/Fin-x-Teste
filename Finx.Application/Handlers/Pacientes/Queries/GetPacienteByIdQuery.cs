using MediatR;
using Finx.Domain.Entities;

namespace Finx.Application.Handlers.Pacientes.Queries;

public record GetPacienteByIdQuery(Guid Id) : IRequest<Paciente?>;
