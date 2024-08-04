using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class TokenRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
