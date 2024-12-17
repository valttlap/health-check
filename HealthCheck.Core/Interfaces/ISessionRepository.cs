using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Interfaces;

public interface ISessionRepository
{
    public Task<Session?> GetById(Guid id);
    public Task<Session> CreateSession(int joinCode);
    public Task<Category> GetCategoryForSession(Guid id);
    public Task<Category> MoveToNextCategory(Guid id);
    public Task<IEnumerable<Vote>> GetSessionVotes(Guid id);
    public Task<int> GetSessionUsersCount(Guid id);
    public Task<int> GetSessionCategoryVoteCount(Guid id, int categoryId);
}
