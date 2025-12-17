using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finx.Domain
{
    public interface IPacienteRepository
    {
        Task<Guid> AddAsync(Paciente paciente);
        Task<Paciente?> GetByIdAsync(Guid id);
        Task<IEnumerable<Paciente>> ListAsync();
        Task UpdateAsync(Paciente paciente);
        Task DeleteAsync(Guid id);
    }
}