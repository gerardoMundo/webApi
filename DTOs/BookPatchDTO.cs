using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class BookPatchDTO
    {
        [Required]
        [StringLength(maximumLength: 250)]
        public string Title { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}
