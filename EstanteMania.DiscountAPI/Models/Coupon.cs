using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstanteMania.DiscountAPI.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [StringLength(30)]
        public string Code { get; set; } = null!;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Discount { get; set; }
    }
}
