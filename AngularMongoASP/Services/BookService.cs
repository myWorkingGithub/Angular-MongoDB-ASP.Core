using System;
using MongoDB.Driver;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AngularMongoASP.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;

namespace AngularMongoASP.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;
        private readonly IMongoDatabase _database;
        private readonly IFileService _fileService;
        private readonly GridFSBucket _bucket;

        public BookService(IBookstoreDatabaseSettings settings, IFileService fileService)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _database = client.GetDatabase(settings.DatabaseName);
            _fileService = fileService;

            _bucket = new GridFSBucket(_database);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
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

        public void Update(string id, Book bookIn) =>
            _books.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}