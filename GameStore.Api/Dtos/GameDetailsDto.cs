// File created by Rudy Liljeberg, August 1st, 2026

namespace GameStore.Api.Dtos;

public record GameDetailsDto(
    int Id,
    string Name,
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate // Parameters for Game Data Transfer Object (DTO)
);
