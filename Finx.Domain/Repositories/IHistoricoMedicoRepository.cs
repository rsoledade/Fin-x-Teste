using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Finx.Domain.Entities;

namespace Finx.Domain.Repositories
{
    public interface IHistoricoMedicoRepository
    {
        Task<Guid> AddAsync(HistoricoMedico historico);
        Task<HistoricoMedico?> GetByIdAsync(Guid id);
        Task<IEnumerable<HistoricoMedico>> ListByPacienteIdAsync(Guid pacienteId);
        Task UpdateAsync(HistoricoMedico historico);
        Task DeleteAsync(Guid id);
    }
}
