using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace entity.General_Entities
{
    [Table("Role")]
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? RoleName { get; set; }

        [Required]
        public int IsActive { get; set; } = 1;

        public virtual ICollection<User>? Users { get; set; }
    }
}
