using System.ComponentModel.DataAnnotations;

namespace UserDataManagingService.Models.Atrributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Value can't be null");
            }

            if(value is IFormFile file)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                if(!_extension.Contains(fileExtension.ToLower()))
                {
                    return new ValidationResult("wrong image file extension");
                }
            }
            return ValidationResult.Success;
        }
        //
        private readonly string[] _extension;
        public FileExtensionAttribute(params string[] extension)
        {
            _extension = extension;
        }
    }
}
