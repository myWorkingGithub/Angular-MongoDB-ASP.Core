using System;
using System.Threading.Tasks;
using AngularMongoASP.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost]
       // [Route("upload")]
        public IActionResult Upload( IFormFile file)
        {
             _fileService.UploadProfilePicture(file);
             return Ok();
        }

       [HttpPost]
       [Route("upload")]
        public async Task<ObjectId>UploadFile(IFormFile file)
        {
            return await _fileService.UploadFile(file);
        }
    }
}
