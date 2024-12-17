using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HealthCheck.API.Authorization;
using HealthCheck.Core.Interfaces;

using Microsoft.AspNetCore.Mvc;

namespace HealthCheck.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController(ISessionService sessionService, ISessionUserService sessionUserService, IJwtTokenService jwtTokenService) : ControllerBase
    {
        private readonly ISessionService _sessionService = sessionService;
        private readonly ISessionUserService _sessionUserService = sessionUserService;
        private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

        public async Task<IActionResult> CreateSession()
        {
            var session = await _sessionService.CreateSession();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(8)
            };

            var token = _jwtTokenService.GenerateToken(session.Id, true, TimeSpan.FromHours(8));

            Response.Cookies.Append("SessionId", session.Id.ToString(), cookieOptions);
            Response.Cookies.Append("IsHost", "true", cookieOptions);

            return Ok(new {joinCode = session.JoinCode, token });
        }

        [HttpPost]
        [Route("join/{joinCode}")]
        public async Task<IActionResult> JoinSession(int joinCode)
        {
            var session = await _sessionService.GetSessionByJoinCode(joinCode);
            if (session is null)
            {
                return NotFound();
            }

            var userId = await _sessionUserService.CreateUser(session.Id);

            var token = _jwtTokenService.GenerateToken(session.Id, userId, TimeSpan.FromHours(8));

            return Ok(new
            {
                token,
                expiresAt = DateTime.UtcNow.AddHours(8)
            });
        }
    }
}
