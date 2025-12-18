using MediatR;
using Finx.Domain.Repositories;

namespace Finx.Application.Handlers.Pacientes.Commands;

public sealed class UpdatePacienteCommandHandler : IRequestHandler<UpdatePacienteCommand>
{
    private readonly IPacienteRepository _repo;

    public UpdatePacienteCommandHandler(IPacienteRepository repo)
    {
        _repo = repo;
    }

    public async Task<Unit> Handle(UpdatePacienteCommand request, CancellationToken cancellationToken)
    {
        var paciente = await _repo.GetByIdAsync(request.Id);
        if (paciente is null) return Unit.Value;

        paciente.Nome = request.Nome;
        paciente.DataNascimento = request.DataNascimento;
        paciente.Contato = request.Contato;

        await _repo.UpdateAsync(paciente);
        return Unit.Value;
    }
}
