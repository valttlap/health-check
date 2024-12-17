
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Services;

public class VoteService(IUnitOfWork unitOfWork) : IVoteService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Vote> CastVote(Guid userId, Guid sessionId, int categoryId, int value)
    {
        return await _unitOfWork.VoteRepository.CreateVote(userId, sessionId, categoryId, value);
    }
}
