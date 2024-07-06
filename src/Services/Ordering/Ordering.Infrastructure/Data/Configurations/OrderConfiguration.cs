using Ordering.Domain.Enums;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id).HasConversion(
                customerId => customerId.Value,
                dbId => OrderId.Of(dbId));
            //builder.HasOne<Customer>()
            //    .WithMany()
            //    .HasForeignKey(o => o.CustomerId)
            //    .IsRequired();
            builder.Property(o => o.Note).HasMaxLength(1000).IsRequired(false);
            builder.HasMany<OrderItem>()
            .WithOne()
            .HasForeignKey(o => o.OrderId)
            .IsRequired();
            builder.HasMany(o => o.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

            builder.ComplexProperty(
          o => o.CustomerId, nameBuilder =>
          {
              nameBuilder.Property(n => n.Value)
              .HasColumnName(nameof(Order.CustomerId));
      
          });

            builder.ComplexProperty(
                o => o.OrderName, nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired(false);
                });

            builder.ComplexProperty(
              o => o.ShippingAddress, addressBuilder =>
              {
                  addressBuilder.Property(a => a.CustomerName)
                  .HasMaxLength(50)
                  .IsRequired();

                   addressBuilder.Property(a => a.Phone)
                  .HasMaxLength(50)
                  .IsRequired();

                  addressBuilder.Property(a => a.Province)
                .HasMaxLength(50);

                  addressBuilder.Property(a => a.District)
                .HasMaxLength(180)
                .IsRequired();

                  addressBuilder.Property(a => a.Ward)
                .HasMaxLength(50);

                  addressBuilder.Property(a => a.DetailAddress)
                  .HasMaxLength(50);

            
              });
            builder.ComplexProperty(
           o => o.BillingAddress, addressBuilder =>
           {
               addressBuilder.Property(a => a.CustomerName)
                .HasMaxLength(50)
                .IsRequired();

               addressBuilder.Property(a => a.Phone)
              .HasMaxLength(50)
              .IsRequired();

               addressBuilder.Property(a => a.Province)
             .HasMaxLength(50);

               addressBuilder.Property(a => a.District)
             .HasMaxLength(180)
             .IsRequired();

               addressBuilder.Property(a => a.Ward)
             .HasMaxLength(50);

               addressBuilder.Property(a => a.DetailAddress)
               .HasMaxLength(50);
           });

            builder.ComplexProperty(
           o => o.Payment, paymentBuilder =>
           {
               paymentBuilder.Property(a => a.CardName)
               .HasMaxLength(50)
               .IsRequired(false); 


               paymentBuilder.Property(a => a.CardNumber)
                 .HasMaxLength(24)
                .IsRequired(false);

               paymentBuilder.Property(a => a.Expiration)
             .HasMaxLength(10)
              .IsRequired(false);

               paymentBuilder.Property(a => a.CVV)
             .HasMaxLength(3)
              .IsRequired(false);

               paymentBuilder.Property(a => a.PaymentMethod);
            
           });
            builder.Property(o=>o.Status)
               .HasDefaultValue(OrderStatus.Pending)
               .HasConversion(
                    s=>s.ToString(),
                    dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));
            builder.Property(o => o.TotalPrice);




        }
    }
}
