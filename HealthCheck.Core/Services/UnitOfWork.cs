using HealthCheck.Core.Interfaces;
using HealthCheck.Core.Repositories;
using HealthCheck.Model.Context;

namespace HealthCheck.Core.Services;

public class UnitOfWork(HealthCheckContext context) : IUnitOfWork
{
    private readonly HealthCheckContext _context = context;
    public ICategoryRepository CategoryRepository => new CategoryRepository(_context);

    public ISessionRepository SessionRepository => new SessionRepository(_context);

    public ISessionUserRepository SessionUserRepository => new SessionUserRepository(_context);

    public IVoteRepository VoteRepository => new VoteRepository(_context);

    public async Task<bool> Complete()
    {
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }
}
