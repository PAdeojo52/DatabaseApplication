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
        }
    }
}
