using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace entity.Business_Entities
{
    public class FileHandlerDTO
    {
        [Required]
        [DisplayName("Image")]
        public IFormFile? File { get; set; }
        public string? Url { get; set; }
    }
}
