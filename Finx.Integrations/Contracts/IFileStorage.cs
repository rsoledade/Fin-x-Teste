namespace Finx.Integrations.Contracts
{
    public interface IFileStorage
    {
        Task DeleteAsync(string id);
        Task<byte[]?> GetAsync(string id);
        Task<string> UploadAsync(string fileName, byte[] content);
    }
}
