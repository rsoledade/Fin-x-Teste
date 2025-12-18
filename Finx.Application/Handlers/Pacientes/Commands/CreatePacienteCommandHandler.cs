using MediatR;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Finx.Application.Handlers.Pacientes.Commands;

public sealed class CreatePacienteCommandHandler : IRequestHandler<CreatePacienteCommand, Guid>
{
    private readonly IPacienteRepository _repo;
    private readonly ILogger<CreatePacienteCommandHandler> _logger;

    public CreatePacienteCommandHandler(IPacienteRepository repo, ILogger<CreatePacienteCommandHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreatePacienteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Creating paciente. Cpf={Cpf} Nome={Nome}", request.Cpf, request.Nome);

            var paciente = new Paciente
            {
                Nome = request.Nome,
                Cpf = request.Cpf,
                DataNascimento = request.DataNascimento,
                DataCadastro = DateTime.UtcNow,
                Contato = request.Contato
            };

            var id = await _repo.AddAsync(paciente);

            _logger.LogInformation("Paciente created. Id={PacienteId} Cpf={Cpf}", id, request.Cpf);
            return id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating paciente. Cpf={Cpf} Nome={Nome}", request.Cpf, request.Nome);
            throw;
        }
    }
}
