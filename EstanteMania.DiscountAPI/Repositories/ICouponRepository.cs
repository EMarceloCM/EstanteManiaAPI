using EstanteMania.DiscountAPI.Models;

namespace EstanteMania.DiscountAPI.Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon> GetCouponByIdAsync(string couponCode);
    }
}
