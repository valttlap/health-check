
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Context;
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Repositories;

public class VoteRepository(HealthCheckContext context) : IVoteRepository
{
    private readonly HealthCheckContext _context = context;

    public async Task<Vote> CreateVote(Guid userId, Guid sessionId, int categoryId, int value)
    {
        if (value < 1 || value > 3)
        {
            throw new InvalidOperationException($"Vote value should be between 1 and 3. Got: {value}");
        }
        var vote = new Vote()
        {
            UserId = userId,
            SessionId = sessionId,
            CategoryId = categoryId,
        };
        await _context.AddAsync(vote);
        await _context.SaveChangesAsync();

        return vote;
    }
}
