namespace HealthCheck.API.Authorization;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using YamlDotNet.Core.Tokens;

public class JwtTokenService : IJwtTokenService
{
    private readonly SymmetricSecurityKey _signingKey;

    public JwtTokenService(IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var secretKey = jwtSection.GetValue<string>("SecretKey");

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    }

    public string GenerateToken(Guid sessionId, Guid userId, TimeSpan expiry)
    {
        return GenerateTokenInternal(sessionId, expiry, userId, false);
    }

    public string GenerateToken(Guid sessionId, bool isHost, TimeSpan expiry)
    {
        return GenerateTokenInternal(sessionId, expiry, null, isHost);
    }

    private string GenerateTokenInternal(Guid sessionId, TimeSpan expiry, Guid? userId, bool isHost = false)
    {
        var claims = new List<Claim>
        {
            new("sessionId", sessionId.ToString())
        };

        if (userId.HasValue)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, userId.Value.ToString()));
        }
        else if (isHost)
        {
            claims.Add(new Claim("isHost", "true"));
        }
        else
        {
            throw new ApplicationException("User must be host or have userId");
        }

        var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.Add(expiry);

        var token = new JwtSecurityToken(
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiration,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
