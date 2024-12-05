using DatabaseApplication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseTestingSolution.DBInitializations
{
    public class DatabaseTestFixture : IDisposable
    {
        public ApplicationDbContext Context { get; private set; }

        public DatabaseTestFixture()
        {
            // Load configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false) // Ensure appsettings.json exists
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(configuration.GetConnectionString("DefaultConnection")) // Use your real database
                .Options;

            // Initialize the database context
            Context = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            // Optional: Clean up the database after tests
            Context.Dispose();
        }
    }
}
