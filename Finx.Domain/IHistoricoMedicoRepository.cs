using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finx.Domain
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
