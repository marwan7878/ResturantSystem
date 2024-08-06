namespace ResturantSystem.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public DateTime OrderingTime { get; set; } = DateTime.Now;

        public DateTime PaymentTime { get; set; }

        public ICollection<Product> Products { get; set; }
    }

}

