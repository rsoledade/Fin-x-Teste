using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finx.Domain;
using Microsoft.EntityFrameworkCore;

namespace Finx.Infrastructure.Repositories
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly FinxDbContext _db;

        public PacienteRepository(FinxDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> AddAsync(Paciente paciente)
        {
            paciente.Id = Guid.NewGuid();
            paciente.DataCadastro = paciente.DataCadastro == default ? DateTime.UtcNow : paciente.DataCadastro;
            _db.Pacientes.Add(paciente);
            await _db.SaveChangesAsync();
            return paciente.Id;
        }

        public async Task<Paciente?> GetByIdAsync(Guid id)
        {
            return await _db.Pacientes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Paciente>> ListAsync()
        {
            return await _db.Pacientes.AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(Paciente paciente)
        {
            _db.Pacientes.Update(paciente);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _db.Pacientes.FindAsync(id);
            if (entity != null)
            {
                _db.Pacientes.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }
    }
}