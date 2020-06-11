using System;
using System.Threading.Tasks;
using AngularMongoASP.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace AngularMongoASP.Controllers
{
   // [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        private readonly FileService _fileService;

        public FileController(FileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
        public async Task<string> Upload([FromForm(Name = "icon")]IFormFile file)
        {
            ObjectId id = await _fileService.UploadFile(file);
            return id.ToString();
        }
        [HttpGet("test")]
        public async Task<string> DownloadFile()
        {
            await _fileService.DownloadFileMongo();
            return "";
        }

       /*public IActionResult Upload(IFormFile file)
       {
          // var myFile = Request.Form.Files[0];


          _fileService.Save(file);
          return Ok();
       }*/

       /*[HttpPost]
       [Route("upload")]
       public async Task<ObjectId>UploadFile(IFormFile file)
       {
           return await _fileService.UploadFile(file);
       }*/

       /*[HttpPost]
       [Route("upload")]
       public string UploadFile(IFormFile file)
       {
           return _fileService.UploadFile(file);
       }*/

       /*
       [HttpPost]
       [Route("upload")]
       public string UploadFile()
       {
           return fileService.UploadFileMongo();
       }
       */

      // POST api/notes/uploadFile
       /*[HttpPost("uploadFile")]
       public async Task<string> UploadFile(IFormFile file)
       {
           return await _fileService.UploadFile(file);
       }*/

       // POST api/notes/uploadFile
       [HttpPost("uploadFile")]
       public async Task<ObjectId> UploadFile(IFormFile file)
       {
           return await _fileService.UploadFile(file);
       }

       // GET api/notes/getFileInfo/d1we24ras41wr
       [HttpGet("getFileInfo/{id}")]
       public Task<String> GetFileInfo(string id)
       {
           return _fileService.GetFileInfo(id);
       }
    }
}
