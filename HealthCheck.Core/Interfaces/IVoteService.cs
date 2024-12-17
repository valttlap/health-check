using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Interfaces;

public interface IVoteService
{
    public Task<Vote> CastVote(Guid userId, Guid sessionId, int categoryId, int value);
}
