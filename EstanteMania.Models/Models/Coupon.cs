namespace EstanteMania.Models.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        public string Code { get; set; } = null!;

        public decimal Discount { get; set; }
    }
}