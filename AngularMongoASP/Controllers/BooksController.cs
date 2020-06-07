using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngularMongoASP.Models;
using AngularMongoASP.Services;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace AngularMongoASP.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]
    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService;
        private readonly IFileService _fileService;

        public BooksController(BookService bookService, IFileService fileService)
        {
            _bookService = bookService;
            _fileService = fileService;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get() => _bookService.Get();

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<Book> Create(Book book)
        {
            var newBook = await _bookService.Create(book);
           // return CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
           return newBook;
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Update(id, bookIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = _bookService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            _bookService.Remove(book.Id);

            return NoContent();
        }
    }
}