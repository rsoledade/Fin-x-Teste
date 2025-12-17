using System.IO;
using System.Threading.Tasks;
using Finx.Integrations.Contracts;
using System;

namespace Finx.Integrations.Adapters
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _basePath;

        public LocalFileStorage(string basePath)
        {
            _basePath = basePath;
            Directory.CreateDirectory(_basePath);
        }

        public async Task<string> UploadAsync(string fileName, byte[] content)
        {
            var id = Guid.NewGuid().ToString();
            var path = Path.Combine(_basePath, id + Path.GetExtension(fileName));
            await File.WriteAllBytesAsync(path, content);
            return id;
        }

        public async Task<byte[]?> GetAsync(string id)
        {
            var files = Directory.GetFiles(_basePath, id + "*", SearchOption.TopDirectoryOnly);
            if (files.Length == 0) return null;
            return await File.ReadAllBytesAsync(files[0]);
        }

        public Task DeleteAsync(string id)
        {
            var files = Directory.GetFiles(_basePath, id + "*", SearchOption.TopDirectoryOnly);
            foreach (var f in files) File.Delete(f);
            return Task.CompletedTask;
        }
    }
}