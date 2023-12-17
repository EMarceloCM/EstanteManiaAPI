using EstanteMania.DiscountAPI.Models;
using EstanteMania.DiscountAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.DiscountAPI.Context
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _context;

        public CouponRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Coupon> GetCouponByIdAsync(string couponCode)
        {
            var coupon = await _context.Coupons.AsNoTracking().FirstOrDefaultAsync(x => x.Code == couponCode);

            return coupon;
        }
    }
}
