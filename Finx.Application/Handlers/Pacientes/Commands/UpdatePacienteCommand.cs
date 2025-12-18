using MediatR;

namespace Finx.Application.Handlers.Pacientes.Commands;

public record UpdatePacienteCommand(Guid Id, string Nome, DateTime DataNascimento, string Contato) : IRequest;
