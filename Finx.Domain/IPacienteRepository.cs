using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finx.Domain
{
    public interface IPacienteRepository
    {
        Task<Guid> AddAsync(Paciente paciente);
        Task<Paciente?> GetByIdAsync(Guid id);
        Task<IEnumerable<Paciente>> ListAsync(int page = 1, int pageSize = 20);
        Task UpdateAsync(Paciente paciente);
        Task DeleteAsync(Guid id);
    }
}
