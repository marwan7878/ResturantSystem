using Microsoft.AspNetCore.Identity;
using ResturantSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
	public class ApplicationUser:IdentityUser
	{
        [Required,MaxLength(100)]
        public string FirstName { get; set; }

        [Required,MaxLength(100)]
        public string LastName { get; set; }
        
        public byte[]? ProfilePicture { get; set; }

        public int? BranchId { get; set; }

        public Branch? Branch { get; set; }
    }
}
