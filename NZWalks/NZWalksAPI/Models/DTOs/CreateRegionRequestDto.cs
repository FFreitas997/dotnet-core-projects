using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTOs;

public class CreateRegionRequestDto
{
    [Required(ErrorMessage = "Name is required")]
    [MaxLength(100, ErrorMessage = "Name must have maximum of 100 characters")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [MinLength(3, ErrorMessage = "Code must have minimum of 3 characters")]
    [MaxLength(3, ErrorMessage = "Code must have minimum of 3 characters")]
    public required string Code { get; set; }

    public string? ImageUrl { get; set; }
}