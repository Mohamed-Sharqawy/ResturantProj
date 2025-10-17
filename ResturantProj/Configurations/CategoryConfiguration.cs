using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResturantProj.Models;

namespace ResturantProj.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            
            builder.ToTable("Categories");

            
            builder.HasKey(c => c.Id);

            
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);

            builder.Property(c => c.IsDeleted)
                .HasDefaultValue(false);

            builder.HasIndex(c => c.IsDeleted);

           
            builder.HasQueryFilter(c => !c.IsDeleted);

            
            builder.HasMany(c => c.MenuItems)
                .WithOne(m => m.Category)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
