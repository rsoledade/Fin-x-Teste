using System;
using MediatR;

namespace Finx.Api.Handlers.Pacientes.Commands
{
    public record CreatePacienteCommand(string Nome, string Cpf, DateTime DataNascimento, string Contato) : IRequest<Guid>;
}
