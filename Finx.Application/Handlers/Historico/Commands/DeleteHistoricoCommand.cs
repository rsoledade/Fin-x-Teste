using MediatR;

namespace Finx.Application.Handlers.Historico.Commands;

public record DeleteHistoricoCommand(Guid PacienteId, Guid Id) : IRequest;
