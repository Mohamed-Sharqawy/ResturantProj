using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantProj.Models;

namespace ResturantProj.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(oi => oi.Id);

            builder.Property(oi => oi.Quantity)
                .IsRequired();

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.Subtotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(oi => oi.PreparationTimeMinutes)
                .IsRequired();

            builder.HasIndex(oi => oi.OrderId);
            builder.HasIndex(oi => oi.MenuItemId);
        }
    }
}
