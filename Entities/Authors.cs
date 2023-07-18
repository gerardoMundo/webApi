namespace WebApi.Entities
{
    public class Authors
    {
        public int Bookid { get; set; }
        public int AuthorId { get; set; }
        public int Sort { get; set; }
        public Books Book { get; set; }
        public Author Author { get; set; }
    }
}
