using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Finx.Domain;

namespace Finx.Infrastructure.Repositories
{
    public class HistoricoMedicoRepository : IHistoricoMedicoRepository
    {
        private readonly FinxDbContext _context;

        public HistoricoMedicoRepository(FinxDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(HistoricoMedico historico)
        {
            historico.Id = Guid.NewGuid();
            _context.HistoricosMedicos.Add(historico);
            await _context.SaveChangesAsync();
            return historico.Id;
        }

        public async Task<HistoricoMedico?> GetByIdAsync(Guid id)
        {
            return await _context.HistoricosMedicos.FindAsync(id);
        }

        public async Task<IEnumerable<HistoricoMedico>> ListByPacienteIdAsync(Guid pacienteId)
        {
            return await _context.HistoricosMedicos
                .Where(h => h.PacienteId == pacienteId)
                .OrderByDescending(h => h.Data)
                .ToListAsync();
        }

        public async Task UpdateAsync(HistoricoMedico historico)
        {
            _context.HistoricosMedicos.Update(historico);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var historico = await _context.HistoricosMedicos.FindAsync(id);
            if (historico != null)
            {
                _context.HistoricosMedicos.Remove(historico);
                await _context.SaveChangesAsync();
            }
        }
    }
}
