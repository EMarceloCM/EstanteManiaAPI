using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EstanteMania.DiscountAPI.DTO_s
{
    public class CouponDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Code { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Discount { get; set; }
    }
}
