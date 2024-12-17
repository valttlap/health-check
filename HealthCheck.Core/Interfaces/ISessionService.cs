using HealthCheck.Core.Dtos;
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Interfaces;

public interface ISessionService
{
    Task<Session> CreateSession();
    Task<Session?> GetSession(Guid id);
    Task<Session?> GetSessionByJoinCode(int joinCode);
    Task<bool> EveryoneVoted(Guid id, int categoryId);
    Task<Category> MoveToNextCategory(Guid id);
    Task<IEnumerable<FinalResultDto>> GetFinalResult(Guid id);


}
