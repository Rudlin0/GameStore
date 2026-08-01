//File created by Rudy Liljeberg, August 1st, 2026

using GameStore.Api.Dtos;

const string GetGameEndpointName = "GetGame"; //Creates constant for identifying games by their name

/** Configures service applications **/
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

/** Configures HTTP request pipeline (Defines what happens when HTTP requests start arriving into application) **/

List<GameDto> games = [
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
];

// GET /games
app.MapGet("/games", () => games); // Maps GET ".../games" to display all games in the database

// GET /games/1, Maps GET ".../games/{id}" to display a game in the database by its id #
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id)) // Expects int id to match "/games/{id}"
    .WithName(GetGameEndpointName); //Assigns unique identifier to this request for use in other requests

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
    GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
    ); // 

    games.Add(game);

    return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
}); // Maps POST ".../games" to add a game to the database as a GameDto object, then returns a 201 Created response containing the details of the added game.

app.Run();
