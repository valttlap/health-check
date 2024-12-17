using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Interfaces;

public interface ISessionUserService
{
    public Task<Guid> CreateUser(Guid sessionId);
    public Task<SessionUser?> GetUserById(Guid id);
    public Task UpdateConnectionId(Guid id, string connectionId);
}
