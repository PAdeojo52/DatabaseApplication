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
                entity.HasIndex(u => u.Email).IsUnique(); // Example: make Email unique
                entity.Property(u => u.FirstName).IsRequired();
                entity.Property(u => u.LastName).IsRequired();
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Items"); // Map to "Items" table
                entity.HasKey(e => e.Id); // Primary key
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired(); // Required field
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.Category).HasColumnName("category");
                 // Required field
                entity.Property(e => e.Price).HasColumnName("price");
                entity.Property(e => e.Photo).HasColumnName("photo").IsRequired(false); // Nullable
                entity.Property(e => e.Creator).HasColumnName("creater");
            });
        

        }





    }
}
