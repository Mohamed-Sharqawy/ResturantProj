using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantProj.Models;

namespace ResturantProj.Configurations
{
    public class DailyInventoryLogConfiguration : IEntityTypeConfiguration<DailyInventoryLog>
    {
        public void Configure(EntityTypeBuilder<DailyInventoryLog> builder)
        {
            builder.ToTable("DailyInventoryLogs");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Date)
                .IsRequired();

            builder.Property(d => d.OrderCount)
                .HasDefaultValue(0);

            builder.HasIndex(d => new { d.MenuItemId, d.Date });
        }
    }
}
