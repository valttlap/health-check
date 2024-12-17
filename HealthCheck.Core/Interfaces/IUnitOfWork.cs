namespace HealthCheck.Core.Interfaces;

public interface IUnitOfWork
{
    public ICategoryRepository CategoryRepository { get; }
    public ISessionRepository SessionRepository { get; }
    public ISessionUserRepository SessionUserRepository { get; }
    public IVoteRepository VoteRepository { get; }

    public Task<bool> Complete();
    public bool HasChanges();
}
