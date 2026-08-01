//File created by Rudy Liljeberg, August 1st, 2026

/** Configures service applications **/

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

/** Configures HTTP request pipeline (Defines what happens when HTTP requests start arriving into application) **/

app.MapGet("/", () => "Hello World!");

app.Run();
