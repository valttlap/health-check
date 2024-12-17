using System.IdentityModel.Tokens.Jwt;

using HealthCheck.Core.Exceptions;
using HealthCheck.Core.Interfaces;

using Microsoft.AspNetCore.SignalR;

namespace HealthCheck.API.Hubs;

public class HealthCheckHub(ISessionService sessionService, ISessionUserService sessionUserService, IVoteService voteService) : Hub
{
    private readonly ISessionService _sessionService = sessionService;
    private readonly ISessionUserService _sessionUserService = sessionUserService;
    private readonly IVoteService _voteService = voteService;

    public override async Task OnConnectedAsync()
    {
        var user = Context.User ?? throw new InvalidOperationException("User is null");

        var sessionIdClaim = user.FindFirst("sessionId") ?? throw new InvalidOperationException("Session ID claim is null");
        var sessionGuid = Guid.Parse(sessionIdClaim.Value);

        var isHostClaim = user.FindFirst("isHost");
        var userIdClaim = user.FindFirst(JwtRegisteredClaimNames.Sub);
        string? hostConnectionId;
        if (isHostClaim?.Value == "true")
        {
            hostConnectionId = Context.ConnectionId;
            await _sessionService.UpdateHostConnectionId(sessionGuid, hostConnectionId);
        }
        else if (userIdClaim is not null)
        {
            var userGuid = Guid.Parse(userIdClaim.Value);
            await _sessionUserService.UpdateConnectionId(userGuid, Context.ConnectionId);
            hostConnectionId = await _sessionService.GetHostConnectionId(sessionGuid);
        }
        else
        {
            throw new InvalidOperationException("User has to have user id or be host");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, sessionIdClaim.Value);


        if (hostConnectionId is not null)
        {
            var userCount = await _sessionService.SessionUserCount(sessionGuid);
            await Clients.Client(hostConnectionId).SendAsync("UserConnected", userCount);
        }

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
        var session = await _sessionService.GetSession(sessionGuid) ?? throw new ApplicationException($"No session found for id: {sessionId}");

        await _voteService.CastVote(userGuid, sessionGuid, categoryId, value);

        if (await _sessionService.EveryoneVoted(sessionGuid, categoryId))
        {
            try
            {
                var nextCategory = await _sessionService.MoveToNextCategory(sessionGuid);
                await Clients.GroupExcept(sessionId, session.HostConnectionId!).SendAsync("NextCategory", nextCategory.Id);
                await Clients.Client(session.HostConnectionId!).SendAsync("NewCategory", nextCategory);
            }
            catch (AlreadyLastCategoryException)
            {
                await Clients.GroupExcept(sessionId, session.HostConnectionId!).SendAsync("SurveyEnded");
                var finalResult = _sessionService.GetFinalResult(sessionGuid);
                await Clients.Client(session.HostConnectionId!).SendAsync("FinalResults", finalResult);
            }
        }

    }
}
