
// File created by Rudy Liljeberg, August 1st, 2026

using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

/** Acts as DB context for this application, meaning it represents a session between our API and the database.
    Can be used to both query and save instances of the entities (in this case, games) into the database. **/
public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{
    public DbSet<Game> Games => Set<Game>(); // Establishes a pointer, called "Games", that when referenced points to the corresponding table in the database of type Game

    public DbSet<Genre> Genres => Set<Genre>(); // Establishes a pointer, called "Genres", that when referenced points to the corresponding table in the database of type Genre

}