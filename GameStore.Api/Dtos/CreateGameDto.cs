// File created by Rudy Liljeberg, August 1st, 2026

using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDto(
    [Required][StringLength(50)] string Name, // Name field is limited to 50 characters max
    [Range(1, 50)] int GenreId, // GenreId field is limited to id values from 1 to 50, inclusive
    [Range(1, 100)] decimal Price, // Price field is limited to values from $1 to $100, inclusive
    DateOnly ReleaseDate
);
