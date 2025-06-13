using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTOs;

public class UpdateWalkRequest
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name must have maximum of 100 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [MaxLength(500, ErrorMessage = "Description must have maximum of 500 characters")]
    public required string Description { get; set; }

    [Required(ErrorMessage = "Length in kilometers is required")]
    [Range(0.1, double.MaxValue, ErrorMessage = "Length must be greater than 0.1 km")]
    public double LengthKm { get; set; }

    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Difficulty ID is required")]
    public Guid DifficultyId { get; set; }

    [Required(ErrorMessage = "Region ID is required")]
    public Guid RegionId { get; set; }
}