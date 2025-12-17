using System.Collections.Generic;
using System.Threading.Tasks;
using Finx.Integrations.Contracts;
using System;

namespace Finx.Integrations.Adapters
{
    public class MockExameClient : IExameClient
    {
        public Task<IEnumerable<ExameDto>> GetExamesByCpfAsync(string cpf)
        {
            var exames = new List<ExameDto>
            {
                new ExameDto { Nome = "Hemograma", Data = DateTime.UtcNow.AddDays(-10), Resultado = "Normal" },
                new ExameDto { Nome = "Glicemia", Data = DateTime.UtcNow.AddDays(-5), Resultado = "Elevada" }
            };
            return Task.FromResult<IEnumerable<ExameDto>>(exames);
        }
    }
}