// File created by Rudy Liljeberg, August 1st, 2026

using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExtensions
{
    /** Executes all pending migrations on the database in order to keep this application and db synced up whenever method is ran**/
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope(); // Creates a disposable services scope
        var dbContext = scope.ServiceProvider
                             .GetRequiredService<GameStoreContext>(); // Retrieves and stores an instance of GameStoreContext in the dbContext variable
        dbContext.Database.Migrate(); // Uses dbContext to access the database and execute all pending migrations
    }
}
