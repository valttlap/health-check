using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Interfaces;

public interface IVoteRepository
{
    public Task<Vote> CreateVote(Guid userId, Guid sessionId, int categoryId, int value);
}
