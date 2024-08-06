using Auth.Models;

namespace ResturantSystem.Models
{
    public class Branch
    {
        public int Id { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string City { get; set; }

        public string ManagerId { get; set; }

        public ApplicationUser Manager { get; set; }

        public ICollection<Table> Tables { get; set; }
        
        public ICollection<ApplicationUser> Employees { get; set; }
        
        public ICollection<Product> Products { get; set; }




    }
}
