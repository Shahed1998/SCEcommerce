using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name="Name")]
        public string? Name { get; set; }

        [Required]
        [Display(Name = "Display Order")]
        [Remote("DisplayOrderAlreadyExist", "Category", HttpMethod = "POST", ErrorMessage = "Display Order already exist")]
        public int? DisplayOrder { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
