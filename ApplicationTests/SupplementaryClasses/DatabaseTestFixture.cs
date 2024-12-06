using System;
using Microsoft.EntityFrameworkCore;
using DatabaseApplication.Data;
using Microsoft.Extensions.Configuration;

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
        Context.Database.CloseConnectionAsync().Wait();
        //Context.Database.CloseConnection();
        // Optional: Clean up the database after tests
        Context.Dispose();

        
    }
}