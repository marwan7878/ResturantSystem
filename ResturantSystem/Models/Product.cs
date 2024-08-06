namespace ResturantSystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool HasIngredients { get; set; }

        public bool Available { get; set; }

        public string Size { get; set; }

        public float Price { get; set; }

        public ICollection<Branch>? Branches { get; set; }

        public ICollection<Product>? Products { get; set; }

        public ICollection<Product>? Ingredients { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}


