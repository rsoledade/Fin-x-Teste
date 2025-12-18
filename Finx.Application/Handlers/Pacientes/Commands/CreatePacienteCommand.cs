using MediatR;

namespace Finx.Application.Handlers.Pacientes.Commands;

public record CreatePacienteCommand(string Nome, string Cpf, DateTime DataNascimento, string Contato) : IRequest<Guid>;
