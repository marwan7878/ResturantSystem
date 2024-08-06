namespace ResturantSystem.Models
{
    public class Table
    {
        public int Id { get; set; }

        public int TableNo { get; set; }

        public bool Status { get; set; }

        public string QR_Link { get; set; }

        public byte[] QR_Image { get; set; }

        public int BranchId { get; set; }

        public Branch Branch { get; set; }
    }
}
