using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public class DiscountContext : DbContext
    {
        public DiscountContext(DbContextOptions<DiscountContext> options)
            :base(options)
        {
        }

        public DbSet<Coupon> Coupons { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon { Id =1, ProductName = "IPhoneX", Description="Iphone Desc", Amount = 12},
                new Coupon { Id = 2, ProductName = "IPhone11", Description = "Iphone Desc", Amount = 12 }

                );
        }
    }
}
