namespace NZWalksAPI.Models.Entities;

public class Walk
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double LengthKm { get; set; }
    public string? ImageUrl { get; set; }
    public Guid DifficultyId { get; set; }
    public Guid RegionId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Region Region { get; set; } = new Region();
    public Difficulty Difficulty { get; set; } = new Difficulty();
}