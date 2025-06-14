using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NZWalksAPI.Repositories.Interface;

namespace NZWalksAPI.Repositories.Implementations;

public class TokenRepository(IConfiguration configuration) : ITokenRepository
{
    public string CreateJwtToken(IdentityUser user, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expireIn = configuration["Jwt:ExpiryInMinutes"];

        var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(double.TryParse(expireIn, out var minutes) ? minutes : 60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}