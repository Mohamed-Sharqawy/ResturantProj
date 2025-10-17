using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantProj.Models;

namespace ResturantProj.Configurations
{
    public class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");

            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.HasIndex(c => c.IsDeleted);

            builder.HasKey(m => m.Id);

            builder.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.Description)
                .HasMaxLength(500);

            builder.Property(m => m.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(m => m.PreparationTimeMinutes)
                .IsRequired();

            builder.Property(m => m.IsAvailable)
                .HasDefaultValue(true);

            builder.Property(m => m.DailyOrderCount)
                .HasDefaultValue(0);

            builder.HasIndex(m => m.CategoryId);
            builder.HasIndex(m => m.IsAvailable);

            
            builder.HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.OrderItems)
                .WithOne(oi => oi.MenuItem)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(m => m.DailyInventoryLogs)
                .WithOne(d => d.MenuItem)
                .HasForeignKey(d => d.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
