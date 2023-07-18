using System.ComponentModel.DataAnnotations;

namespace WebApi.CustomValidations
{
    public class IsFirstLetterUppercaseAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString())) 
            {
                return ValidationResult.Success;
            }

            var firstLetter = value.ToString()[0].ToString();

            if(firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser en mayúscula");
            }

            return ValidationResult.Success;
        }
    }
}
