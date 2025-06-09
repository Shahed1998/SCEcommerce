using Microsoft.AspNetCore.Http;
using Models.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Models.BusinessEntities
{
    public class Test
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Display(Name = "Image")]
        [AllowedFileExtensionsAttribute(new[] { ".jpg", ".jpeg", ".png", ".gif" }, 5)]
        public IFormFile? FormFile { get; set; }
    }
}
