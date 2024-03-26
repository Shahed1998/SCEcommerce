using System.ComponentModel.DataAnnotations;

namespace entity.Business_Entities
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; } = String.Empty;

        [Required]
        public string Password { get; set; } = String.Empty;
    }
}
