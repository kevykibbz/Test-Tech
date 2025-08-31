using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public sealed class TechTestDbContextFactory : IDesignTimeDbContextFactory<TechTestDbContext>
{
    public TechTestDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TechTestDbContext>();
        
        // For now, use the direct connection string since we don't have LocalDB
        // In production, this would be read from configuration
        var connectionString = "Server=lawvu.techtest.sqlserver;Initial Catalog=LawVuTechTestDB;User ID=SA;Password=LawVu@2022!!";
        
        optionsBuilder.UseSqlServer(connectionString);
        
        return new TechTestDbContext(optionsBuilder.Options);
    }
}
