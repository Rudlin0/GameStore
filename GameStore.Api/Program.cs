// File created by Rudy Liljeberg, August 1st, 2026

using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args); // Creates a new instance of the WebApplicationBuilder class, which is used to configure and build the web application

builder.Services.AddValidation(); // Adds validation services to builder

var connString = "Data Source=GameStore.db"; // Connection string for accessing the database (GameStore.db, in this case)
builder.Services.AddSqlite<GameStoreContext>(connString); // Adds GameStoreContext (instance of DbContext) as a service (essentially the interface layer) between our application and the database

var app = builder.Build(); // Builds the application and prepares it to handle incoming HTTP requests

app.MapGamesEndpoint(); // Configures the HTTP request pipeline to handle requests related to the games database

app.MigrateDb(); // Executes migrations so that application and database data models match before starting application proper

app.Run(); // Starts the application and begins listening for incoming HTTP requests
