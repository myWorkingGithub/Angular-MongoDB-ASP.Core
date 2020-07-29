using System.Collections.Generic;
using AngularMongoASP.Models;
using AngularMongoASP.Services;
using Microsoft.AspNetCore.Mvc;

namespace AngularMongoASP.Controllers {
    [ApiController]
    [Route ("[controller]")]
    public class MyBooksController : ControllerBase {
        private readonly MyBookService _myBookService;

        public MyBooksController (MyBookService myBookService) {
            _myBookService = myBookService;
        }

        [HttpGet]
        public ActionResult<List<Book>> Get () =>
            _myBookService.Get ();

        [HttpGet ("{id:length(24)}", Name = "GetMyBook")]
        public ActionResult<Book> Get (string id) {
            var book = _myBookService.Get (id);

            if (book == null) {
                return NotFound ();
            }

            return book;
        }

        [HttpPost]
        public ActionResult<Book> Create (Book book) {
            _myBookService.Create (book);

            return CreatedAtRoute ("GetMyBook", new { id = book.Id.ToString () }, book);
        }

        [HttpPut ("{id:length(24)}")]
        public IActionResult Update (string id, Book bookIn) {
            var book = _myBookService.Get (id);

            if (book == null) {
                return NotFound ();
            }

            _myBookService.Update (id, bookIn);

            return NoContent ();
        }

        [HttpDelete ("{id:length(24)}")]
        public IActionResult Delete (string id) {
            var book = _myBookService.Get (id);

            if (book == null) {
                return NotFound ();
            }

            _myBookService.Remove (book.Id);

            return NoContent ();
        }
    }
}