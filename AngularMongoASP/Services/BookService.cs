using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using AngularMongoASP.Data;
using AngularMongoASP.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;

namespace AngularMongoASP.Services
{
    public class BookService
    {
        private readonly DataContext _dataContext = null;

        public BookService(IDatabaseSettings databaseSettings, IFileService fileService)
        {
            _dataContext = new DataContext(databaseSettings);
        }

        public List<Book> Get() =>
            _dataContext.Books.Find(book => true).ToList();

        public Book Get(string id) =>
            _dataContext.Books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _dataContext.Books.InsertOne(book);
            return book;
        }

        public async Task<ObjectId> UploadFile(IFormFile file)
        {
            try
            {
                var stream = file.OpenReadStream();
                var filename = file.FileName;
                return await _dataContext.Bucket.UploadFromStreamAsync(filename, stream);
            }
            catch (Exception ex)
            {
                return new ObjectId(ex.ToString());
            }
        }

        public void Update(string id, Book bookIn) =>
            _dataContext.Books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _dataContext.Books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _dataContext.Books.DeleteOne(book => book.Id == id);
    }
}