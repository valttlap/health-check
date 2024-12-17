using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using HealthCheck.Core.Exceptions;
using HealthCheck.Core.Interfaces;
using HealthCheck.Model.Entities;

using Microsoft.AspNetCore.SignalR;

namespace HealthCheck.API.Hubs;

public class HealthCheckHub(ISessionService sessionService, ISessionUserService sessionUserService, IVoteService voteService) : Hub
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly ISessionUserService _sessionUserService = sessionUserService;
    private readonly IVoteService _voteService = voteService;

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public async Task CastVote(int value, int categoryId)
    {
        // Get the user Id
        var user = Context.User ?? throw new InvalidOperationException("User is null");
        var userId = user.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new InvalidOperationException("User id is null");
        var userGuid = Guid.Parse(userId);
        var sessionId = user.FindFirst("SessionId")?.Value ?? throw new InvalidOperationException("Session ID is null");
        var sessionGuid = Guid.Parse(sessionId);

        if (await _sessionUserService.GetUserById(userGuid) is null) throw new ApplicationException($"No user found for id: {userId}");
        if (await _sessionService.GetSession(sessionGuid) is null) throw new ApplicationException($"No session found for id: {sessionId}");

        await _voteService.CastVote(userGuid, sessionGuid, categoryId, value);

        if (await _sessionService.EveryoneVoted(sessionGuid, categoryId))
        {
            try
            {
                var nextCategory = await _sessionService.MoveToNextCategory(sessionGuid);
                await Clients.GroupExcept(sessionId, "TODO: HOST CONNECTION STRING").SendAsync("NextCategory", nextCategory.Id);
                await Clients.Client("TODO: HOST").SendAsync("NewCategory", nextCategory);
            }
            catch (AlreadyLastCategoryException)
            {
                await Clients.GroupExcept(sessionId, "TODO: HOST CONNECTION STRING").SendAsync("SurveyEnded");
                var finalResult = _sessionService.GetFinalResult(sessionGuid);
                await Clients.Client("TODO: HOST").SendAsync("FinalResults", finalResult);
            }
        }
    }
}
