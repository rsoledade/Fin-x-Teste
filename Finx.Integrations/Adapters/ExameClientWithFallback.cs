using Finx.Integrations.Contracts;

namespace Finx.Integrations.Adapters
{
    public class ExameClientWithFallback : IExameClient
    {
        private readonly ExameHttpClient _httpClient;
        private readonly MockExameClient _mock;

        public ExameClientWithFallback(ExameHttpClient httpClient, MockExameClient mock)
        {
            _httpClient = httpClient;
            _mock = mock;
        }

        public async Task<IEnumerable<ExameDto>> GetExamesByCpfAsync(string cpf)
        {
            try
            {
                return await _httpClient.GetExamesByCpfAsync(cpf);
            }
            catch
            {
                return await _mock.GetExamesByCpfAsync(cpf);
            }
        }
    }
}
