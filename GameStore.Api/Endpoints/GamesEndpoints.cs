// File created by Rudy Liljeberg, August 1st, 2026

using System;

namespace GameStore.Api.Endpoints;

using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints
{

    const string GetGameEndpointName = "GetGame"; // Creates constant for identifying games by their name

    /** Configures HTTP request pipeline (Defines what happens when HTTP requests start arriving into application) **/
    public static void MapGamesEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/games"); // Creates a group of endpoints for handling requests related to games, with the base URL "/games"

        // GET /games, Maps "GET .../games" to asynchronously display all games currently in the database
        group.MapGet("/", async (GameStoreContext dbContext) 
            => await dbContext.Games
                              .Include(game => game.Genre) // Ensures that the game's Genre will be included in this selection                            
                              .Select(game => new GameSummaryDto(
                                game.Id,
                                game.Name,
                                game.Genre!.Name,
                                game.Price,
                                game.ReleaseDate
                              )) // Projects each game in our Games table in the database as a new GameSummaryDto object
                              .AsNoTracking() // Tells EF Core not to track any changes from the selection process
                              .ToListAsync()); // Asynchronously creates a list of type GameSummaryDto out of the projected game objects

        // GET /games/1, Maps "GET .../games/{id}" to asynchronously display a game in the database by its id #
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id); // Asynchronously finds the game with the specified id in the database

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                ) // Returns a 404 Not Found response if the game with the specified id is not found in the database, otherwise returns a 200 OK response with details of the specified game as a new GameDetailsDto object
            );
        })
            .WithName(GetGameEndpointName); // Assigns unique identifier to this request for use in other requests

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) => // NOTE: Always include "async" and "await" keywords in endpoints to denote the use of asynchronous methods during execution
        {
            Game game = new()
            {
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            }; // Defines game model as a means of providing information for new game entry into database

            dbContext.Games.Add(game); // Asks EntityFrameworkCore to begin tracking that a new game needs to be inserted into the database
            await dbContext.SaveChangesAsync(); // Asynchronously saves all changes made in this context to the database by translating any pending changes in the DbContext change tracker into SQL statements that the database can understand, before then executing said statements

            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            ); // Creates a new object of type GameDetailsDto using information provided 

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto); // Returns a 201 Created response containing the details of the added game, and includes a Location header with the URI of the newly created game resource
        }); // Maps "POST .../games" to asynchronously add a game to the database as an instance of the model Game object, 
            // expects CreateGameDto object to define the new game model to insert into the database,
            // as well as an instance of GameStoreContext in order to store game into the database using said context

        // PUT /games/1
        group.MapPut("/{id}", async (
            int id, 
            UpdateGameDto updatedGame, 
            GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id); // Asynchronously finds the game with the specified id in the database

            if (existingGame is null)
            {
                return Results.NotFound(); // Returns a 404 Not Found response if the specified game does not yet exist in the database
            }

            existingGame.Name = updatedGame.Name; // Updates the name of the existing game at the specified id in the database to that of updatedGame
            existingGame.GenreId = updatedGame.GenreId; // Updates the genre id of the existing game at the specified id in the database to that of updatedGame
            existingGame.Price = updatedGame.Price; // Updates the price of the existing game at the specified id in the database to that of updatedGame
            existingGame.ReleaseDate = updatedGame.ReleaseDate; // Updates the release date of the existing game at the specified id in the database to that of updatedGame

            await dbContext.SaveChangesAsync(); // Asynchronously saves all changes made in this context to the database by translating any pending changes in the DbContext change tracker into SQL statements that the database can understand, before then executing said statements

            return Results.NoContent(); // Returns a 204 No Content response to indicate that the update was successful, but that there is no content to return in the response body
        }); // Maps "PUT .../games/{id}" to asynchronously update a game in the database by its id #, expects int id to match "/games/{id}" and UpdateGameDto object to update the game with

        // DELETE /games/1
        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
           await dbContext.Games
                          .Where(game => game.Id == id) // Filters all games in the database by the id provided in the method header.
                          .ExecuteDeleteAsync(); // Asynchronously deletes any games that match the id provided in the method header. 
                                                 // NOTE: No SaveChangesAsync() is needed as ExecuteDeleteAsync() will delete the game from the database on its own (assuming the game already existed in the database to begin with).
                        

            return Results.NoContent(); // Returns a 204 No Content response whether or not the game with the specified id was found and removed from the database
        }); // Maps "DELETE .../games/{id}" to asynchronously delete a game in the database by its id # regardless of whether or not there is a game with an id # in the database that matches the one provided, expects int id to match "/games/{id}"
    }
}