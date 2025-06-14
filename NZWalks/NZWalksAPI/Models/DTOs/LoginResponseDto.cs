namespace NZWalksAPI.Models.DTOs;

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required DateTime ExpiresAt { get; set; }
}