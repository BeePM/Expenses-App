using System.ComponentModel.DataAnnotations;

namespace Expenses.API.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is null)
            {
                return new ValidationResult("Identity number is required");
            }

            var id = Convert.ToInt32(value);
            if (id <= 0)
            {
                return new ValidationResult("Identity must be a number greater than 0");
            }

            return ValidationResult.Success;
        }
    }
}
