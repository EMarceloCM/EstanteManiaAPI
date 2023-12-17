using EstanteMania.DiscountAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EstanteMania.DiscountAPI.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 1,
                Code = "PROMO_10",
                Discount = 10
            });

            modelBuilder.Entity<Coupon>().HasData(new Coupon
            {
                Id = 2,
                Code = "PROMO_20",
                Discount = 20
            });
        }
    }
}
