using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Models.Attributes
{
    public class AllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;
        private readonly long _maxFileSizeInMB;

        public AllowedFileExtensionsAttribute(string[] allowedExtensions, long maxFileSizeInMB)
        {
            _allowedExtensions = allowedExtensions;
            _maxFileSizeInMB = maxFileSizeInMB * 1024 * 1024;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file != null)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || !_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"Unsupported file type. Allowed: {string.Join(", ", _allowedExtensions)}");
                }

                if (file.Length > _maxFileSizeInMB)
                {
                    return new ValidationResult($"File size exceeds {_maxFileSizeInMB}MB.");
                }
            }

            return ValidationResult.Success;

        }
    }
}
