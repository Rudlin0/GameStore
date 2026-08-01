// File created by Rudy Liljeberg, August 1st, 2026

namespace GameStore.Api.Models;

public class Game
{
    public int Id { get; set; }

    public required string Name { get; set; } // Compiler will force Name to be defined whenever called

    public Genre? Genre { get; set; } // Nullable constructor for object of type Genre

    public int GenreId { get; set; } // Constructor for Genre Id. Can be called separate from Genre, meaning Genre object type doesn't have to be loaded each time this is called.

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }
}
