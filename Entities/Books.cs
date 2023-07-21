using System.ComponentModel.DataAnnotations;

namespace WebApi.Entities
{
    public class Books
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:250)]
        public string Title { get; set; }
        public List<Comment> Comments { get; set; }
        public List<AuthorsBooks> Authors { get; set; }
    }
}
