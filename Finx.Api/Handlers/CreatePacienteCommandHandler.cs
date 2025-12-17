using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Finx.Domain;
using Finx.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Finx.Api.Handlers
{
    public class CreatePacienteCommandHandler : IRequestHandler<CreatePacienteCommand, Guid>
    {
        private readonly FinxDbContext _db;

        public CreatePacienteCommandHandler(FinxDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> Handle(CreatePacienteCommand request, CancellationToken cancellationToken)
        {
            var paciente = new Paciente
            {
                Id = Guid.NewGuid(),
                Nome = request.Nome,
                Cpf = request.Cpf,
                DataNascimento = request.DataNascimento,
                DataCadastro = DateTime.UtcNow,
                Contato = request.Contato
            };

            _db.Pacientes.Add(paciente);
            await _db.SaveChangesAsync(cancellationToken);

            return paciente.Id;
        }
    }
}