using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantProj.Models;

namespace ResturantProj.Configurations
{
    public class DeliveryAddressConfiguration : IEntityTypeConfiguration<DeliveryAddress>
    {
        public void Configure(EntityTypeBuilder<DeliveryAddress> builder)
        {
            builder.ToTable("DeliveryAddresses");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.CustomerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(d => d.FullAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.City)
                .HasMaxLength(100);

            builder.Property(d => d.PostalCode)
                .HasMaxLength(20);

            builder.Property(d => d.DeliveryInstructions)
                .HasMaxLength(500);

            builder.HasIndex(d => d.OrderId)
                .IsUnique();
        }
    }
}
