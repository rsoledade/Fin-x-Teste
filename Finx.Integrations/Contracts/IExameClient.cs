namespace Finx.Integrations.Contracts
{
    public interface IExameClient
    {
        Task<IEnumerable<ExameDto>> GetExamesByCpfAsync(string cpf);
    }
}
