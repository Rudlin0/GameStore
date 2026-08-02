// File created by Rudy Liljeberg, August 1st, 2026

using GameStore.Api.Models;
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

    /** Adds GameStore.db file as this application's database, 
        while also establishing GameStoreContext as a service between this app and the database, 
        as well as seeding the Genres table within said database **/
    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        var connString =  builder.Configuration.GetConnectionString("GameStore"); // Establishes connection string for accessing the database (GameStore.db, in this case)
        
        // DbContext has a Scoped service lifetime because:
        // 1. It ensures that a new instance of DbContext is created per request
        // 2. DB connections are a limited and expensive resource
        // 3. DbContext is not thread-safe. Scoped avoids concurrency issues
        // 4. Makes it easier to manage transactions and ensure data consistency
        // 5. Reusing a DbContext instance can lead to increased memory usage (Scoped = reduces memory overhead, with potential performance improvements as a result)

        builder.Services.AddSqlite<GameStoreContext>( // Registers our DbContext (called GameStoreContext here) with a scope service lifetime
            connString,
            optionsAction: options => options.UseSeeding((context, _) => // NOTE: UseSeeding() takes in two parameters, the second of which we discard using an underscore here
            {
                if(!context.Set<Genre>().Any()) // If GameStoreContext does not have a list of genres set yet...
                {
                    context.Set<Genre>().AddRange(
                        new Genre { Name = "Fighting" },
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Platformer" },
                        new Genre { Name = "Racing" },
                        new Genre { Name = "Sports" }
                    ); // ...Then begin tracking and saving to this context the instances of game genres listed here.

                    context.SaveChanges(); // Save changes made to GameStoreContext to the database as well
                }
            }) // Seeds the database when the application starts
        ); // Adds GameStoreContext (instance of DbContext) as a service (essentially the interface layer) between our application and the database
    }
}
