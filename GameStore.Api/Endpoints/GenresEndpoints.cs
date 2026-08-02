// File created by Rudy Liljeberg, August 2nd, 2026

using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Endpoints;

public static class GenresEndpoints
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres");

        // GET /genres
        group.MapGet("/", async (GameStoreContext dbContext) =>
            await dbContext.Genres
                           .Select(Genre => new GenreDto(Genre.Id, Genre.Name)) // Projects each genre in our Genres table in the database as a new GenreDto object
                           .AsNoTracking() // Tells EF Core not to track any changes from the selection process
                           .ToListAsync()  // Asynchronously creates a list of type GenreDto out of the projected Genre objects
            ); // Maps "GET .../genres" to asynchronously display all genres currently in the database
    }
}
