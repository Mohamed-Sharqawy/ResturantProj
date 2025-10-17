using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantProj.Models;

namespace ResturantProj.Configurations
{
    public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.ToTable("OrderStatusHistories");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.FromStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(h => h.ToStatus)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(h => h.ChangedAt)
                .IsRequired();

            builder.Property(h => h.Notes)
                .HasMaxLength(500);

            builder.HasIndex(h => h.OrderId);
            builder.HasIndex(h => h.ChangedAt);
        }
    }
}
