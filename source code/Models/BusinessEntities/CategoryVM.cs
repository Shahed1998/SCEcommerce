using System.ComponentModel.DataAnnotations;

namespace Models.BusinessEntities
{
  
    public class CategoryVM
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }

        public int Total { get; set; }

        public ICollection<ProductVM> Products { get; set; } = new List<ProductVM>();
    }
}
