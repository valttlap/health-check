
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Context;
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Repositories;

public class SessionUserRepository(HealthCheckContext context) : ISessionUserRepository
{
    private readonly HealthCheckContext _context = context;

    public async Task<SessionUser> CreateUser(Guid sessionId)
    {
        var newUser = new SessionUser();
        await _context.SessionUsers.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    public async Task<SessionUser?> GetUserById(Guid id)
    {
        return await _context.SessionUsers.FindAsync(id);
    }

    public async Task UpdateConnectionId(Guid id, string connectionId)
    {
        var user = await _context.SessionUsers.FindAsync(id) ?? throw new InvalidOperationException($"User with id: {id} not found");
        user.ConnectionId = connectionId;
        await _context.SaveChangesAsync();
    }
}
