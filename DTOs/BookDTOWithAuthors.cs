namespace WebApi.DTOs
{
    public class BookDTOWithAuthors: BookWithID
    {
        public List<AuthorWithID> Authors { get; set; }
    }
}
