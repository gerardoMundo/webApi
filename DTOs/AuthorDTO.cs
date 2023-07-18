using System.ComponentModel.DataAnnotations;
using WebApi.CustomValidations;

namespace WebApi.DTOs
{
    public class AuthorDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [IsFirstLetterUppercase]
        [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} debe tener máximo: {1} caracteres.")]
        public string Name { get; set; }
        
    }
}
