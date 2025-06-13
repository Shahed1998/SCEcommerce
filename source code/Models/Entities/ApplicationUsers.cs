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

        public string? Country { get; set; }
        
        public string? State { get; set; }

        public int? PostalCode { get; set; }
        
        public string? PresentAddress { get; set; }

        public string? PermanentAddress { get; set; }

    }
}
