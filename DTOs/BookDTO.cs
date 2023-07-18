using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class BookDTO
    {
        [Required]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
    }
}
