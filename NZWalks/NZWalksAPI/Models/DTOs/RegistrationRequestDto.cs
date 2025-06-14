using System.ComponentModel.DataAnnotations;

namespace NZWalksAPI.Models.DTOs;

public class RegistrationRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 100 characters long.")]
    public required string FirstName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 100 characters long.")]
    public required string LastName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
    public required string Password { get; set; }

    [Required] public required string[] Roles { get; set; }
}