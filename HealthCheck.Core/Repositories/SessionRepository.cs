
using HealthCheck.Core.Exceptions;
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Context;
using HealthCheck.Model.Entities;

using Microsoft.EntityFrameworkCore;

namespace HealthCheck.Core.Repositories;

public class SessionRepository(HealthCheckContext context) : ISessionRepository
{
    private readonly HealthCheckContext _context = context;

    public async Task<Session> CreateSession(int joinCode)
    {
        var newSession = new Session()
        {
            JoinCode = joinCode
        };
        await _context.Sessions.AddAsync(newSession);
        await _context.SaveChangesAsync();
        return newSession;
    }

    public async Task<Session?> GetById(Guid id)
    {
        return await _context.Sessions.FindAsync(id);
    }

    public async Task<Session?> GetByJoinCode(int joinCode)
    {
        return await _context.Sessions.FirstOrDefaultAsync(s => s.JoinCode == joinCode);
    }

    public async Task<Category> GetCategoryForSession(Guid id)
    {
        return await _context.Sessions
            .Where(s => s.Id == id)
            .Select(s => s.CurrentCategory)
            .FirstAsync();
    }

    public async Task<string?> GetHostConnectionId(Guid id)
    {
        return await _context.Sessions.Where(s => s.Id == id).Select(s => s.HostConnectionId).FirstAsync();
    }

    public async Task<int> GetSessionCategoryVoteCount(Guid id, int categoryId)
    {
        return await _context.Sessions
            .Where(s => s.Id == id)
            .SelectMany(s => s.Votes)
            .Where(v => v.CategoryId == categoryId)
            .CountAsync();
    }

    public async Task<int> GetSessionUsersCount(Guid id)
    {
        return await _context.Sessions
            .Where(s => s.Id == id)
            .SelectMany(s => s.SessionUsers)
            .CountAsync();
    }

    public async Task<IEnumerable<Vote>> GetSessionVotes(Guid id)
    {
        return await _context.Sessions
            .Where(s => s.Id == id)
            .SelectMany(s => s.Votes)
            .Include(v => v.Category)
            .ToListAsync();
    }


    public async Task<Category> MoveToNextCategory(Guid id)
    {
        var session = await _context.Sessions.FirstAsync(s => s.Id == id);

        var nextCategoryId = session.CurrentCategoryId + 1;
        var nextCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == nextCategoryId)
            ?? throw new AlreadyLastCategoryException();

        session.CurrentCategoryId = nextCategoryId;
        await _context.SaveChangesAsync();

        return nextCategory;
    }

    public async Task UpdateHostConnectionId(Guid id, string connectionId)
    {
        var session = await _context.Sessions.FindAsync(id) ?? throw new InvalidOperationException($"Session with {id} not found");
        session.HostConnectionId = connectionId;
        await _context.SaveChangesAsync();
    }
}
