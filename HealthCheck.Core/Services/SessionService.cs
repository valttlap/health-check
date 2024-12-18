
using HealthCheck.Core.Dtos;
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Services;

public class SessionService(IUnitOfWork unitOfWork) : ISessionService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Session> CreateSession()
    {
        var sessionCode = CreateSessionCode();
        return await _unitOfWork.SessionRepository.CreateSession(sessionCode);
    }

    public async Task<bool> EveryoneVoted(Guid id, int categoryId)
    {
        var userCount = await _unitOfWork.SessionRepository.GetSessionUsersCount(id);
        var voteCount = await _unitOfWork.SessionRepository.GetSessionCategoryVoteCount(id, categoryId);

        return userCount == voteCount;
    }

    public async Task<IEnumerable<FinalResultDto>> GetFinalResult(Guid id)
    {
        var votes = await _unitOfWork.SessionRepository.GetSessionVotes(id);

        var results = votes
            .GroupBy(v => v.Category)
            .Select(group => new FinalResultDto
            {
                CategoryTitleEn = group.Key.TitleEn,
                CategoryTitleFi = group.Key.TitleFi,
                GoodAnswerCount = group.Count(v => v.VoteValue == 3),
                NeutralAnswerCount = group.Count(v => v.VoteValue == 2),
                BadAnswerCount = group.Count(v => v.VoteValue == 1),
                AverageAnswer = group.Any()
                    ? (int)Math.Round(group.Average(v => v.VoteValue), MidpointRounding.AwayFromZero)
                    : 0
            })
            .ToList();

        return results;
    }


    public Task<Session?> GetSession(Guid id)
    {
        return _unitOfWork.SessionRepository.GetById(id);
    }

    public Task<Category> MoveToNextCategory(Guid id)
    {
        return _unitOfWork.SessionRepository.MoveToNextCategory(id);
    }

    private static int CreateSessionCode()
    {
        Random random = new();

        int number = random.Next(100000, 1000000);
        return number;
    }

    public async Task<Session?> GetSessionByJoinCode(int joinCode)
    {
        return await _unitOfWork.SessionRepository.GetByJoinCode(joinCode);
    }

    public async Task UpdateHostConnectionId(Guid id, string connectionId)
    {
        await _unitOfWork.SessionRepository.UpdateHostConnectionId(id, connectionId);
    }

    public async Task<int> SessionUserCount(Guid id)
    {
        return await _unitOfWork.SessionRepository.GetSessionUsersCount(id);
    }

    public async Task<string?> GetHostConnectionId(Guid id)
    {
        return await _unitOfWork.SessionRepository.GetHostConnectionId(id);
    }
}
