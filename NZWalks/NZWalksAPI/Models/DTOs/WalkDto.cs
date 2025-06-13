namespace NZWalksAPI.Models.DTOs;

public class WalkDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public double LengthKm { get; set; }
    public string? ImageUrl { get; set; }
    public Guid DifficultyId { get; set; }
    public Guid RegionId { get; set; }

    public required RegionDto Region { get; set; }
    public required DifficultyDto Difficulty { get; set; }
}