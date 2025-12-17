using System;
using MediatR;
using Finx.Api.DTOs;

namespace Finx.Api.Handlers
{
    public record UpdatePacienteCommand(Guid Id, string Nome, string Cpf, DateTime? DataNascimento, string? Contato) : IRequest<bool>;
}
