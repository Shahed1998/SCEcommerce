using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Models.BusinessEntities
{
  
    public class CategoryVM
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Display Order")]
        [Remote("DisplayOrderAlreadyExist", "Category", HttpMethod = "POST", AdditionalFields = nameof(Id), ErrorMessage = "Display Order already exist")]
        public int? DisplayOrder { get; set; }

        public int Total { get; set; }

        public int page { get; set; }
        public int pageSize { get; set; }

        public ICollection<ProductVM> Products { get; set; } = new List<ProductVM>();
        
    }
}
