using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Finx.Integrations.Contracts;

namespace Finx.Integrations.Adapters
{
    public class ExameHttpClient : IExameClient
    {
        private readonly HttpClient _http;

        public ExameHttpClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<ExameDto>> GetExamesByCpfAsync(string cpf)
        {
            // Example external call - endpoint should return JSON array of ExameDto
            var resp = await _http.GetFromJsonAsync<IEnumerable<ExameDto>>($"/exames/{cpf}");
            return resp ?? new List<ExameDto>();
        }
    }
}