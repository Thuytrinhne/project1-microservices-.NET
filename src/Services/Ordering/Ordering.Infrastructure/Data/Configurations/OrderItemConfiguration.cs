using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(c => c.Id).HasConversion(
               customerId => customerId.Value,
               dbId => OrderItemId.Of(dbId));
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            //builder.HasOne<Product>()
            //    .WithMany()
            //    .HasForeignKey(x => x.ProductId);
            builder.HasOne<Order>()
               .WithMany()
               .HasForeignKey(x => x.OrderId);

            builder.ComplexProperty(
                o => o.ProductId, nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(OrderItem.ProductId));

                });
          
        }
    }
}
