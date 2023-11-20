using System.ComponentModel.DataAnnotations;

namespace UserDataManagingService.Models.Atrributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file) 
            { 
            if(file.Length > _maxFileSize)
                {
                    return new ValidationResult(ErrorMsgIfFileTooBig());
                }
            }
            return ValidationResult.Success;
        }

        private string ErrorMsgIfFileTooBig()
        {
            return $"file is bigger than limit ({_maxFileSize/1048576} Mb)";
        }

        //
        private readonly int _maxFileSize;

        public MaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }
    }
}
