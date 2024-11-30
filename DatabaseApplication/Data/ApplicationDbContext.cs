using DatabaseApplication.EntityModels;
using Microsoft.EntityFrameworkCore;
using static DatabaseApplication.NewFolder.NewUser;

namespace DatabaseApplication.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
        {
        }

        // Define DbSet for User entity
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; } // Map to the "items" table


        // Override OnModelCreating if you need to configure entity relationships, indexes, or constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Example: Configuring additional properties or constraints if needed
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.email).IsUnique(); // Example: make Email unique
                entity.Property(u => u.first_name).IsRequired();
                entity.Property(u => u.last_name).IsRequired();
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Items"); // Table name
                entity.HasKey(e => e.id); // Primary key
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Category).HasColumnName("category");
                entity.Property(e => e.Stock).HasColumnName("stock");
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.Photo).HasColumnName("photo");
                entity.Property(e => e.Creator).HasColumnName("creater");
            });

        }





    }
}
