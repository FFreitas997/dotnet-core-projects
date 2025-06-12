namespace NZWalksAPI.Models.DTOs;

public class UpdateRegionRequest
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}