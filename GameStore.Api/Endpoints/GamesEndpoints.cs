// File created by Rudy Liljeberg, August 1st, 2026

using System;

namespace GameStore.Api.Endpoints;

using GameStore.Api.Dtos;

public static class GamesEndpoints
{

    const string GetGameEndpointName = "GetGame"; // Creates constant for identifying games by their name

    private static readonly List<GameDto> games = [
        new (
            1,
            "Street Fighter II",
            "Fighting",
            19.99M,
            new DateOnly(1992, 7, 15)),
        new (
            2,
            "Final Fantasy VII Rebirth",
            "RPG",
            69.99M,
            new DateOnly(2024, 2, 29)),
        new (
            3,
            "Astro Bot",
            "Platformer",
            59.99M,
            new DateOnly(2024, 9, 6))
    ]; // Creates a static list of GameDto objects to represent the games in the database

    /** Configures HTTP request pipeline (Defines what happens when HTTP requests start arriving into application) **/
    public static void MapGamesEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("/games"); // Creates a group of endpoints for handling requests related to games, with the base URL "/games"

        // GET /games
        group.MapGet("/", () => games); // Maps "GET .../games" to display all games in the database

        // GET /games/1, Maps "GET .../games/{id}" to display a game in the database by its id #
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id); // Finds the game with the specified id in the database

            return game is null ? Results.NotFound() : Results.Ok(game); // Returns a 404 Not Found response if the game with the specified id is not found in the database, otherwise returns a 200 OK response with details of the specified game
        })
            .WithName(GetGameEndpointName); // Assigns unique identifier to this request for use in other requests

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            ); // Creates a new GameDto object with details provided from newGame, and assigns it a new id based on the current number of games in the database + 1

            games.Add(game); // Adds the newly created GameDto object to the database

            return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game); // Returns a 201 Created response containing the details of the added game, and includes a Location header with the URI of the newly created game resource
        }); // Maps "POST .../games" to add a game to the database as a GameDto object, expects CreateGameDto object to add the game with

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id); // Finds the index of the game with the specified id in the database

            if (index == -1)
            {
                return Results.NotFound(); // Returns a 404 Not Found response if the game with the specified id is not found in the database
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            ); // Updates the game with the specified id in the database with the new values from updatedGame. Not threat-safe.

            return Results.NoContent(); // Returns a 204 No Content response to indicate that the update was successful, but that there is no content to return in the response body
        }); // Maps "PUT .../games/{id}" to update a game in the database by its id #, expects int id to match "/games/{id}" and UpdateGameDto object to update the game with

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id); // Removes the game with the specified id from the database

            return Results.NoContent(); // Returns a 204 No Content response whether or not the game with the specified id was found and removed from the database
        }); // Maps "DELETE .../games/{id}" to delete a game in the database by its id # regardless of whether or not there is a game with an id # in the database that matches the one provided, expects int id to match "/games/{id}"
    }
}