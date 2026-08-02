// File created by Rudy Liljeberg, August 1st, 2026

namespace GameStore.Api.Models;

/** Model for the Genre object type, including Id and Name attributes **/
public class Genre
{
    public int Id { get; set; }

    public required string Name { get; set; } // Compiler will force Name to be defined whenever this method is called
}
