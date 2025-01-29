using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AdminPanel.Dal.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // Path to the AdminPanel.Web project where appsettings.json resides
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "AdminPanel.Web");

        // Build configuration
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath) // Point to the AdminPanel.Web directory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        Console.WriteLine($"Base path: {basePath}");
        Console.WriteLine($"Expected appsettings.json path: {Path.Combine(basePath, "appsettings.json")}");

        // Retrieve the connection string
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("The connection string 'DefaultConnection' was not found in the appsettings.json file.");
        }

        // Configure DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(
            connectionString,
            sqlServerOptionsAction: b => b.MigrationsAssembly("AdminPanel.Dal")); // Ensure migrations are created in the DAL project

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}