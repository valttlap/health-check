using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Interfaces;

public interface ISessionUserRepository
{
    public Task<SessionUser?> GetUserById(Guid id);
    public Task<SessionUser> CreateUser(Guid sessionId);
}
