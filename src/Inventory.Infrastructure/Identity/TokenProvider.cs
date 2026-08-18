using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Inventory.Application.Common.Interfaces;
using Inventory.Application.Features.Identity.Dtos;
using Inventory.Domain.Common.Results;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.Infrastructure.Identity;

public sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public Task<Result<TokenResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default)
    {
        var settings = configuration.GetSection("JwtSettings");
        var secret = settings["Secret"];
        var issuer = settings["Issuer"];
        var audience = settings["Audience"];

        if (string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
            return Task.FromResult<Result<TokenResponse>>(Error.Failure("Jwt.ConfigurationMissing", "JWT configuration is incomplete."));

        var expires = DateTime.UtcNow.AddMinutes(int.TryParse(settings["TokenExpirationInMinutes"], out var minutes) ? minutes : 60);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.UserId),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer, audience, claims, expires: expires, signingCredentials: credentials);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult<Result<TokenResponse>>(new TokenResponse(accessToken, expires));
    }
}
