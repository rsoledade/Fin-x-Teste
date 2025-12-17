using System.Threading.Tasks;

namespace Finx.Integrations.Contracts
{
    public interface IFileStorage
    {
        Task<string> UploadAsync(string fileName, byte[] content);
        Task<byte[]?> GetAsync(string id);
        Task DeleteAsync(string id);
    }
}