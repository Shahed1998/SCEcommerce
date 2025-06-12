using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class ApplicationUsers : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }

        [Required] 
        public string? LastName { get;set; }

    }
}
