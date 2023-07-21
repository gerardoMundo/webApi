using System.ComponentModel.DataAnnotations;
using WebApi.CustomValidations;

namespace WebApi.Entities
{
    public class Author
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]//Validaciones en el model
        [IsFirstLetterUppercase] // Validación personalizada
        [StringLength(maximumLength:120, ErrorMessage = "El campo {0} debe tener máximo: {1} caracteres.")]
        public string Name { get; set; }
        public List<AuthorsBooks> Authors { get; set; }

    }
}
