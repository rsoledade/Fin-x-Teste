using MediatR;

namespace Finx.Application.Handlers.Historico.Commands;

public record UpdateHistoricoCommand(Guid Id, Guid PacienteId, string Diagnostico, string Exame, string Prescricao, DateTime Data) : IRequest;
