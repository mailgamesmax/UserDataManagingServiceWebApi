using System.ComponentModel.DataAnnotations;

namespace UserDataManagingService.Models.Atrributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class NotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //if (value is string str && string.IsNullOrWhiteSpace(str))
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return new ValidationResult("required fields can't be empty");
            }

            return ValidationResult.Success;
        }
        //
/*        private readonly string _fieldValue;
        public NotNullOrWhiteSpaceAttribute(string fieldValue)
        {
            _fieldValue = fieldValue;
        }*/
    }
}
