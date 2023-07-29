namespace WebApi.DTOs
{
    public class AuthorDTOWithBooks: AuthorWithID
    {
        public List<BookWithID> Books { get; set; }
    }
}
