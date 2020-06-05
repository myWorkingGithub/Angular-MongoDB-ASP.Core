using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularMongoASP.Services
{
    public interface IFileService
    {
        Task<string> Save(IFormFile file);
    }

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public FileService(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string> Save(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName)?.ToLower();
            var fileName = Guid.NewGuid() + extension;

            var dir = Path.Combine(_hostEnvironment.WebRootPath ?? _hostEnvironment.ContentRootPath,
                "uploads");

            var absolute = Path.Combine(dir, fileName);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            await using var writer = new FileStream(absolute, FileMode.CreateNew);
            await file.CopyToAsync(writer);

            return absolute;
        }

        public async Task<string> UploadProfilePicture([FromForm(Name = "uploadedFile")] IFormFile file, long userId)
        {
            /*if (file == null || file.Length == 0)
                throw new UserFriendlyException("Please select profile picture");*/

            var folderName = Path.Combine("Resources", "ProfilePics");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var uniqueFileName = $"{userId}_profilepic.png";
            var dbPath = Path.Combine(folderName, uniqueFileName);

            using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return dbPath;
        }

    }
}
