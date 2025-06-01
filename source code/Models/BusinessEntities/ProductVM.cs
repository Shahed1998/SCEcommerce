using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Models.BusinessEntities
{
    public class ProductVM
    {
        public int Id { get; set; }

        [Required]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public long? Price { get; set; }

        private int? _discountPercentage;

        [Display(Name = "Discount %")]
        public int? DiscountPercentage
        {
            get
            {
                return _discountPercentage;
            }
            
            set
            {
                _discountPercentage = value ?? 0;
            }
        } 

        public string? ImageUrl { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        public CategoryVM? Category { get; set; }

        public List<SelectListItem> CategoryList { get; set; } = new List<SelectListItem>();

    }
}
