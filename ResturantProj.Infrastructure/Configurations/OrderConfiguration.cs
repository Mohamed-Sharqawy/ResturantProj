using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantProj.Models;

namespace ResturantProj.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(50);

            // Enum properties stored as integers
            builder.Property(o => o.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(o => o.OrderType)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(o => o.PaymentMethod)
                .HasConversion<int>();

            builder.Property(o => o.PaymentStatus)
                .HasConversion<int>();

            
            builder.Property(o => o.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.TaxAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.Total)
                .HasColumnType("decimal(18,2)");

            builder.Property(o => o.StatusChangedAt)
                .IsRequired();

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.CreatedAt);
            builder.HasIndex(o => o.OrderType);

            
            builder.HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.DeliveryAddress)
                .WithOne(d => d.Order)
                .HasForeignKey<DeliveryAddress>(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(o => o.OrderStatusHistories)
                .WithOne(h => h.Order)
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
