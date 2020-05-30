namespace AngularMongoASP.Models
{

    public interface IBooksCollections
    {
        public string Books { get; set; }
        public string MyBooks { get; set; }
    }
    public interface IBookstoreDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string MyBooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class BookstoreDatabaseSettings : IBookstoreDatabaseSettings
    {
        public string BooksCollectionName { get; set; }

        public string MyBooksCollectionName { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}