namespace WebApi.Entities
{
    public class AuthorsBooks
    {
        public int BooksId { get; set; }
        public int AuthorId { get; set; }
        public int Sort { get; set; }
        public Books Books { get; set; }
        public Author Authors { get; set; }
    }
}
