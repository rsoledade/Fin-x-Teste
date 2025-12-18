using MediatR;
using Finx.Domain.Entities;

namespace Finx.Application.Handlers.Pacientes.Queries;

public record GetPacientesQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<Paciente>>;
