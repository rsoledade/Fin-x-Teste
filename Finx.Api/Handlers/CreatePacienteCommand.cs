using System;
using MediatR;
using Finx.Domain;

namespace Finx.Api.Handlers
{
    public record CreatePacienteCommand(string Nome, string Cpf, DateTime DataNascimento, string Contato) : IRequest<Guid>;
}