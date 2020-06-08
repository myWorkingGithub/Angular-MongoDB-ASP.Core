using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AngularMongoASP.Data;
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
     //   string Save(IFormFile file);
        Task<string> UploadProfilePicture(IFormFile file);
       // string UploadFile(IFormFile file);

        Task<ObjectId> UploadFile(IFormFile file);

        Task<String> GetFileInfo(string id);
        string UploadFileMongo();
        string UploadFileFromAStreamMongo();
        Task DownloadFileMongo();
    }

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly DataContext _dataContext = null;
        private readonly UploadFile _uploadFile = new UploadFile();

        public FileService(IWebHostEnvironment hostEnvironment, IDatabaseSettings databaseSettings)
        {
            _hostEnvironment = hostEnvironment;
            _dataContext = new DataContext(databaseSettings);
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

        public string UploadFileMongo()
        {
            byte[] source = File.ReadAllBytes("/home/anton/WorkSharp/My Mongo Project/Angular-MongoDB-ASP.Core/AngularMongoASP/Uploads/123.png");
            ObjectId id =  _dataContext.Bucket.UploadFromBytes("123.png", source);
            return id.ToString();
        }

        public string UploadFileFromAStreamMongo()
        {
            Stream stream = File.Open("/home/anton/WorkSharp/My Mongo Project/Angular-MongoDB-ASP.Core/AngularMongoASP/Uploads/123.png", FileMode.Open);
            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument()
                {
                    {"author", "Anton Po"},
                    {"year", 2020}
                }
            };
            var id = _dataContext.Bucket.UploadFromStream("/home/anton/WorkSharp/My Mongo Project/Angular-MongoDB-ASP.Core/AngularMongoASP/Uploads/123.png", stream, options);
            return id.ToString();
        }

        public async Task DownloadFileMongo()
        {
            var client = new MongoClient("mongodb://localhost:27017");

            var database = client.GetDatabase("BookstoreDb");

            var gridFsBucketOptions = new GridFSBucketOptions()
            {
                BucketName = "images",
                ChunkSizeBytes = 1048576, // 1МБ
            };

            var bucket = new GridFSBucket(database, gridFsBucketOptions);

            var filter = Builders<GridFSFileInfo<ObjectId>>

                .Filter.Eq(x => x.Filename, "123.png");


            var searchResult = await bucket.FindAsync(filter);

            var fileEntry = searchResult.FirstOrDefault();

            byte[] content = await bucket.DownloadAsBytesAsync(fileEntry.Id);

            File.WriteAllBytes("123.png", content);
        }

        public async Task<ObjectId> UploadFile(IFormFile file)
        {
            try
            {
                Stream stream = File.Open("/home/anton/WorkSharp/My Mongo Project/Angular-MongoDB-ASP.Core/AngularMongoASP/Uploads/123.png", FileMode.Open);

              //  var stream = file.OpenReadStream();
              //  var filename = file.FileName;
                var extension = Path.GetExtension(file.FileName)?.ToLower();
                var fileName = Guid.NewGuid() + extension;
              return await _dataContext.Bucket.UploadFromStreamAsync(fileName, stream);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                return new ObjectId(ex.ToString());
            }
        }

        /*public async Task<string> UploadFile(IFormFile file)
        {
            try
            {
                Stream stream = File.Open("sample.pdf", FileMode.Open);
              //  var stream = file.OpenReadStream();
                var filename = file.FileName;
                ObjectId id = await _dataContext.Bucket.UploadFromStreamAsync(filename, stream);
                return id.ToString();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                return "Not Found";
            }
        }  */

        /*public async Task<ObjectId> UploadFile(IFormFile file)
        {
            try
            {
                var stream = file.OpenReadStream();
                var filename = file.FileName;
                return await _dataContext.Bucket.UploadFromStreamAsync(filename, stream);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                return new ObjectId(ex.ToString());
            }
        }*/

        public async Task<String> GetFileInfo(string id)
        {
            GridFSFileInfo info = null;
            var objectId = new ObjectId(id);
            try
            {
                using (var stream = await _dataContext.Bucket.OpenDownloadStreamAsync(objectId))
                {
                    info = stream.FileInfo;
                }
                return info.Filename;
            }
            catch (Exception)
            {
                return "Not Found";
            }
        }

        /*public string Save(IFormFile file)
        {
            try
            {
                string folderName = "Uploads";
                string webRootPath = _hostEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);

                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }

                if (file.Length > 0)
                {
                    string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    _uploadFile.WriteImagePath(fullPath);
                    Console.WriteLine(_uploadFile.ImagePath);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
                return "Upload Successful. " + newPath + "/" + file.FileName;
            }
            catch (System.Exception ex)
            {
                return "Upload Failed: " + ex.Message;
            }
        }*/

        /*public async Task<string> Save(IFormFile file)
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
        }*/
        //  public async Task<string> UploadProfilePicture([FromForm(Name = "uploadedFile")] IFormFile file)
        public async Task<string> UploadProfilePicture(IFormFile file)
        {
            /*if (file == null || file.Length == 0)
                throw new UserFriendlyException("Please select profile picture");*/

            var folderName = Path.Combine("Uploads", "Images");
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
