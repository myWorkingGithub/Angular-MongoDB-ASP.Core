using System.Collections.Generic;
using AngularMongoASP.Data;
using AngularMongoASP.Models;
using MongoDB.Driver;

namespace AngularMongoASP.Services
{
    public class MyBookService
    {
        private readonly DataContext _dataContext = null;

        public MyBookService(IDatabaseSettings databaseSettings)
        {
            _dataContext = new DataContext(databaseSettings);
        }

        public List<Book> Get() =>
            _dataContext.MyBooks.Find(book => true).ToList();

        public Book Get(string id) =>
            _dataContext.MyBooks.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            _dataContext.MyBooks.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn) =>
            _dataContext.MyBooks.ReplaceOne(book => book.Id == id, bookIn);

        public void Remove(Book bookIn) =>
            _dataContext.MyBooks.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _dataContext.MyBooks.DeleteOne(book => book.Id == id);
    }
}