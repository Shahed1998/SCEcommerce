using System.ComponentModel.DataAnnotations;

namespace Models.BusinessEntities
{
  
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string? Name { get; set; }

        [Display(Name = "Display Order")]
        public int? DisplayOrder { get; set; }

        public int Total { get; set; }

        public ICollection<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
