using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using TableTennisAPI.Services.Statistics;
using TableTennisShared.DTO.Dashboard;

namespace TableTennisAPI.Controllers;

[Authorize]
[ApiController]
[Route("stats")]
public class StatisticController(IStatisticsService statisticsService) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardStatsDto>> GetDashBoardStatistics()
    {
        var userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            User.FindFirstValue(JwtRegisteredClaimNames.Sub);

        if (!int.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }

        var stats = await statisticsService.GetDashboardStatsAsync(userId);
        return Ok(stats);
    }
}