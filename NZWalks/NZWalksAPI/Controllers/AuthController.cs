using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.CustomActionFilters;
using NZWalksAPI.Models.DTOs;
using NZWalksAPI.Repositories.Interface;

namespace NZWalksAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    UserManager<IdentityUser> userManager,
    ILogger<RegionsController> logger,
    IConfiguration configuration,
    ITokenRepository tokenRepository) : ControllerBase
{
    [HttpPost]
    [Route("Register")]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto request)
    {
        logger.LogInformation("Registering new user: {Email}", request.Email);

        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email,
            EmailConfirmed = true, // Assuming email confirmation is handled elsewhere
            PhoneNumber = null // Assuming phone number is optional
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            logger.LogError("User registration failed: {Errors}",
                string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // Assign roles if provided
        if (request.Roles.Any())
        {
            var res = await userManager.AddToRolesAsync(user, request.Roles);
            if (!res.Succeeded)
            {
                logger.LogError("Failed to assign roles: {Errors}",
                    string.Join(", ", res.Errors.Select(e => e.Description)));
                return BadRequest(new { Errors = res.Errors.Select(e => e.Description) });
            }
        }

        return Ok(new { Message = "User registered successfully" });
    }

    [HttpPost]
    [Route("Login")]
    [ValidateModel]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        logger.LogInformation("User login attempt: {Email}", request.Email);

        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            logger.LogWarning("Login failed: User not found for email {Email}", request.Email);
            return Unauthorized(new { Message = "Invalid email or password" });
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);

        if (!result)
        {
            logger.LogWarning("Login failed: Incorrect password for email {Email}", request.Email);
            return Unauthorized(new { Message = "Invalid email or password" });
        }

        var roles = await userManager.GetRolesAsync(user);

        if (!roles.Any())
        {
            logger.LogWarning("Login failed: User {Email} has no assigned roles", request.Email);
            return Unauthorized(new { Message = "User has no assigned roles" });
        }

        var generatedToken = tokenRepository.CreateJwtToken(user, roles.ToList());

        var expireIn = configuration["Jwt:ExpiryInMinutes"];

        var response = new LoginResponseDto
        {
            Token = generatedToken,
            ExpiresAt = DateTime.Now.AddMinutes(double.TryParse(expireIn, out var minutes) ? minutes : 60)
        };

        return Ok(response);
    }
}