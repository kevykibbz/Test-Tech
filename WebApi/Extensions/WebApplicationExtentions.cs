using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions;

public static class WebApplicationExtentions
{
    public static IApplicationBuilder ApplyDatabaseMigrations(this WebApplication app, bool redeployDatabase)
    {
        if (app is null)
        {
            throw new ArgumentNullException(nameof(app));
        }

        using var scope = app.Services.CreateScope();

        try
        {
            using var dbContext = scope.ServiceProvider.GetRequiredService<TechTestDbContext>();

            if (redeployDatabase)
            {
                dbContext.Database.EnsureDeleted();
            }

            dbContext.Database.Migrate();
            return app;
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating or seeding the database. Make sure to set the docker-compose project as your startup project and run the WebApi with docker-compose");
            throw;
        }
    }
}
