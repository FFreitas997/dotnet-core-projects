using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTOs;

public class UploadImageRequestDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    [MinLength(3, ErrorMessage = "Name must be at least 3 characters long.")]
    public required string Name { get; set; }

    [Required] public required IFormFile File { get; set; }

    public string? Description { get; set; }
}