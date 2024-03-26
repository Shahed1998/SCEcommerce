using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace entity.general_entities
{
    [Table("Category")]
    public class Category
    {
        [Key]
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

        [Required]
        public int IsActive { get; set; } = 1;

    }
}
