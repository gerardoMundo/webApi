using Microsoft.AspNetCore.Identity;

namespace WebApi.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int BooksId { get; set; }
        public Books Books { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
    }
}
