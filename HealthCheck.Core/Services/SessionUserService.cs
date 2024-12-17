
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Entities;

namespace HealthCheck.Core.Services;

public class SessionUserService(IUnitOfWork unitOfWork) : ISessionUserService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Guid> CreateUser(Guid sessionId)
    {
        var newUser = await _unitOfWork.SessionUserRepository.CreateUser(sessionId);
        return newUser.Id;
    }

    public async Task<SessionUser?> GetUserById(Guid id)
    {
        return await _unitOfWork.SessionUserRepository.GetUserById(id);
    }

    public async Task UpdateConnectionId(Guid id, string connectionId)
    {
        await _unitOfWork.SessionUserRepository.UpdateConnectionId(id, connectionId);
    }
}
