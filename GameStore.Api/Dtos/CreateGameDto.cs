// File created by Rudy Liljeberg, August 1st, 2026

using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record CreateGameDto(
    [Required][StringLength(50)] string Name, // Name field is limited to 50 characters max
    [Required][StringLength(20)] string Genre, // Genre field is limited to 20 characters max
    [Range(1, 100)] decimal Price, // Price field is limited to values from $1 to $100, inclusive
    DateOnly ReleaseDate
);
