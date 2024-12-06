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
        var connectionString = "Host=aws-0-us-west-1.pooler.supabase.com;Port=6543;Database=postgres;Username=postgres.fgplemjtxynlxftuwsit;Password=2WM6ViuWDOsaSRKB;Trust Server Certificate=true;Timeout=30;CommandTimeout=60";

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(connectionString)
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