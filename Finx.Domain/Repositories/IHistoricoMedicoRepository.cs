using Finx.Domain.Entities;

namespace Finx.Domain.Repositories
{
    public interface IHistoricoMedicoRepository
    {
        Task DeleteAsync(Guid id);
        Task UpdateAsync(HistoricoMedico historico);
        Task<HistoricoMedico?> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(HistoricoMedico historico);
        Task<IEnumerable<HistoricoMedico>> ListByPacienteIdAsync(Guid pacienteId);
    }
}
