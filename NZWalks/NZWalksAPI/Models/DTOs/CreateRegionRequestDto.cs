namespace NZWalksAPI.Models.DTOs;

public class CreateRegionRequestDto
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string? ImageUrl { get; set; }
}