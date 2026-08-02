// File created by Rudy Liljeberg, August 1st, 2026

namespace GameStore.Api.Dtos;

// A DTO is a contract between the client and server since it represents
// a shared agreement about how data will be transferred and used.

public record GameSummaryDto(
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate // Parameters for Game Data Transfer Object (DTO)
);
