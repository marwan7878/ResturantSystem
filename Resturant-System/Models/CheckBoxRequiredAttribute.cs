using Auth.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class CheckBoxRequiredAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var roles = (List<RoleViewModel>)value;
            if(roles.Any(r=>r.IsSelected))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Select at least one role!!");
        }
    }
}
