using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using entity.general_entities;
using entity.Business_Entities;

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
        public FileHandlerDTO? Image { get; set; }

        #region Mapping
        public static explicit operator CategoryDTO(Category category) => new CategoryDTO
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            Image = new FileHandlerDTO {  Url = category.Image },
        };
        

        public static explicit operator Category(CategoryDTO dto) => new Category
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Image = dto.Image?.Url,
        };
        #endregion
    }
}
