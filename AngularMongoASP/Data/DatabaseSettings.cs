namespace AngularMongoASP.Data {
    public interface IDatabaseSettings {
        public string BooksCollectionName { get; set; }
        public string NotesCollectionName { get; set; }
        public string MyBooksCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public class DatabaseSettings : IDatabaseSettings {
        public string NotesCollectionName { get; set; }
        public string BooksCollectionName { get; set; }

        public string MyBooksCollectionName { get; set; }

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}