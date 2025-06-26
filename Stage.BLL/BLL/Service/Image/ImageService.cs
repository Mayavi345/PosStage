using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stage.BLL.BLL.Service
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName);
        Task<Stream> GetImageAsync(string imageUrl);
        Task<bool> DeleteImageAsync(string imageUrl);
    }
    public class LocalFileImageService : IImageService
    {
        private readonly string _uploadDirectory;

        public LocalFileImageService(string uploadDirectory)
        {
            _uploadDirectory = uploadDirectory;
            EnsureDirectoryExists(_uploadDirectory);
        }

        public Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageStream.CopyTo(fileStream);
            }

            return Task.FromResult(filePath);
        }

        public Task<Stream> GetImageAsync(string imageUrl)
        {
            var filePath = imageUrl; // Assuming imageUrl is the file path
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            return Task.FromResult<Stream>(fileStream);
        }

        public Task<bool> DeleteImageAsync(string imageUrl)
        {
            var filePath = imageUrl; // Assuming imageUrl is the file path

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private void EnsureDirectoryExists(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
    internal class ImageService
    {
    }
}
