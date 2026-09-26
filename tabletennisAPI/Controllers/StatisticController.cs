using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.Services.Statistics;
using TableTennisShared.DTO.Dashboard;

namespace TableTennisAPI.Controllers;

[Authorize]
[ApiController]
[Route("stats")]
public class StatisticController(IStatisticsService statisticsService)
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardStatsDto>> GetDashBoardStatistics() => await statisticsService.GetDashboardStatsAsync();
}
