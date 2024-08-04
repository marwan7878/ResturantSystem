using System.ComponentModel.DataAnnotations;

namespace Auth.Models
{
    public class AssignRole
    {
        public string UserId { get; set; }
        
        public string Role { get; set; }
    }
}
