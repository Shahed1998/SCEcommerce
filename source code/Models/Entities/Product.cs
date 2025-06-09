using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    [Table("Product")]
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? UniqueProductId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Title { get; set; }

        [StringLength(50)]
        public string? Description { get; set; }

        [Required]
        public long? Price { get; set; }

        [Required]
        [Display(Name = "Discount")]
        public int? DiscountPercentage { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

    }
}
