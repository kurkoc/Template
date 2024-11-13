using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

using TemplateSolution.Infrastructure.Authentication.Abstractions;

namespace TemplateSolution.Infrastructure.Authentication.Concretes;

public class JwtService(JwtSettings jwtSettings, TimeProvider timeProvider) : IJwtService
{
    public async Task<dynamic> GenerateToken(Guid userId, string firstname, string lastname, string email)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Sid, userId.ToString()),
            new Claim(ClaimTypes.Name, firstname),
            new Claim(ClaimTypes.Surname, lastname),
            new Claim(ClaimTypes.Email, email)
        ];

        DateTime utcNow = timeProvider.GetUtcNow().UtcDateTime;
        DateTime expireDate = utcNow.AddMinutes(jwtSettings.Expires);

        var token = new JwtSecurityToken(
            jwtSettings.Issuer,
            jwtSettings.Audience,
            claims,
            expires: expireDate,
            notBefore: DateTime.UtcNow,
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256));

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return await Task.FromResult(new { AccessToken = tokenString, ExpireDate = expireDate });
    }
}