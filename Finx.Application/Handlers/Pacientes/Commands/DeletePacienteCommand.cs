using MediatR;

namespace Finx.Application.Handlers.Pacientes.Commands;

public record DeletePacienteCommand(Guid Id) : IRequest;
