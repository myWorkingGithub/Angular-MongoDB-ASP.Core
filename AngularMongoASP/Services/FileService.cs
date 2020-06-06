using System;
using System.IO;
using System.Threading.Tasks;
using AngularMongoASP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace AngularMongoASP.Services
{
    public interface IFileService
    {
        Task<string> Save(IFormFile file);
        Task<string> UploadProfilePicture(IFormFile file);
        Task<ObjectId> UploadFile(IFormFile file);
    }

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly GridFSBucket _bucket;


        public FileService(IWebHostEnvironment hostEnvironment, IBookstoreDatabaseSettings settings)
        {
            _hostEnvironment = hostEnvironment;
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            var gridFsBucket = new GridFSBucketOptions()
            {
                BucketName = "images",
                ChunkSizeBytes = 1048576, // 1МБ
            };
            _bucket = new GridFSBucket(database, gridFsBucket);
        }
        public async Task<ObjectId> UploadFile(IFormFile file)
        {
            try
            {
                var stream = file.OpenReadStream();
                var filename = file.FileName;
                return await _bucket.UploadFromStreamAsync(filename, stream);
            }
            catch (Exception ex)
            {
                return new ObjectId(ex.ToString());
            }
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

        public async Task<string> UploadProfilePicture([FromForm(Name = "uploadedFile")] IFormFile file)
        {
            /*if (file == null || file.Length == 0)
                throw new UserFriendlyException("Please select profile picture");*/

            var folderName = Path.Combine("Uploads", "BooksIcons");
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            var uniqueFileName = folderName;

            var dbPath = Path.Combine(folderName, uniqueFileName);

            using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return dbPath;
        }


    }
}
