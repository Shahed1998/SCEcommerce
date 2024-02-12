using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using entity.general_entities;

namespace entity.business_entities
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Name")]
        public string? Name { get; set; }
        [Required]
        [MaxLength(100)]
        [DisplayName("Description")]
        public string? Description { get; set; }
        [Required]
        [DisplayName("Image")]
        public string? Image { get; set; }

        public string? Modified { get; set; }

        #region Mapping
        public static explicit operator CategoryDTO(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                Image = category.Image,
                Modified = $"I am {category.Id} and {category.Name}"
            };
        }



        public static explicit operator Category(CategoryDTO dto)
        {
            return new Category
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Image = dto.Image,
            };
        }
        #endregion
    }
}
