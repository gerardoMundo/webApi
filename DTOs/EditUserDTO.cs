using System.ComponentModel.DataAnnotations;

namespace WebApi.DTOs
{
    public class EditUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
