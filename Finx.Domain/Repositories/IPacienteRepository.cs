using Finx.Domain.Entities;

namespace Finx.Domain.Repositories
{
    public interface IPacienteRepository
    {
        Task DeleteAsync(Guid id);
        Task UpdateAsync(Paciente paciente);
        Task<Paciente?> GetByIdAsync(Guid id);
        Task<Guid> AddAsync(Paciente paciente);
        Task<IEnumerable<Paciente>> ListAsync(int page = 1, int pageSize = 20);
    }
}
