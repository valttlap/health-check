namespace HealthCheck.API.Authorization;

public interface IJwtTokenService
{
    string GenerateToken(Guid sessionId, Guid userId, TimeSpan expiry);
    string GenerateToken(Guid sessionId, bool isHost, TimeSpan expiry);
}
