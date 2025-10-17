using Microsoft.EntityFrameworkCore;
using ResturantProj.Models;

namespace ResturantProj.ResContext
{
    public class MyResContext : DbContext
    {
        public MyResContext(DbContextOptions<MyResContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public DbSet<DailyInventoryLog> DailyInventoryLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MyResContext).Assembly);

            //SeedData(modelBuilder);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseModel && (
            e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseModel)entry.Entity;

                if(entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.UpdatedAt = DateTime.UtcNow;
                }

                else if(entry.State == EntityState.Modified)
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
               new Category
               {
                   Id = 1,
                   Name = "Appetizers",
                   IsActive = true,
                   IsDeleted = false,
                   CreatedAt = DateTime.Now,
                   UpdatedAt = DateTime.Now
               },
               new Category
               {
                   Id = 2,
                   Name = "Main Courses",
                   IsActive = true,
                   IsDeleted = false,
                   CreatedAt = DateTime.Now,
                   UpdatedAt = DateTime.Now
               },
               new Category
               {
                   Id = 3,
                   Name = "Desserts",
                   IsActive = true,
                   IsDeleted = false,
                   CreatedAt = DateTime.Now,
                   UpdatedAt = DateTime.Now
               },
               new Category
               {
                   Id = 4,
                   Name = "Beverages",
                   IsActive = true,
                   IsDeleted = false,
                   CreatedAt = DateTime.Now,
                   UpdatedAt = DateTime.Now
               }
           );

            // Seed MenuItems
            modelBuilder.Entity<MenuItem>().HasData(
                new MenuItem
                {
                    Id = 1,
                    Name = "Spring Rolls",
                    Description = "Crispy vegetable spring rolls",
                    Price = 5.99m,
                    PreparationTimeMinutes = 10,
                    CategoryId = 1,
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new MenuItem
                {
                    Id = 2,
                    Name = "Grilled Chicken",
                    Description = "Juicy grilled chicken breast",
                    Price = 12.99m,
                    PreparationTimeMinutes = 25,
                    CategoryId = 2,
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new MenuItem
                {
                    Id = 3,
                    Name = "Beef Steak",
                    Description = "Premium beef steak",
                    Price = 18.99m,
                    PreparationTimeMinutes = 30,
                    CategoryId = 2,
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new MenuItem
                {
                    Id = 4,
                    Name = "Chocolate Cake",
                    Description = "Rich chocolate layer cake",
                    Price = 6.99m,
                    PreparationTimeMinutes = 5,
                    CategoryId = 3,
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new MenuItem
                {
                    Id = 5,
                    Name = "Fresh Orange Juice",
                    Description = "Freshly squeezed orange juice",
                    Price = 3.99m,
                    PreparationTimeMinutes = 3,
                    CategoryId = 4,
                    IsAvailable = true,
                    DailyOrderCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            );
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
