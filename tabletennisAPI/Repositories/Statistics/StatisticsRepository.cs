using Microsoft.EntityFrameworkCore;
using TableTennisAPI.Data;
using TableTennisShared.DTO.Dashboard;

namespace TableTennisAPI.Repositories.Statistics;

public class StatisticsRepository(DatabaseContext dbContext) : IStatisticsRepository
{
    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var dashboardStats = new DashboardStatsDto
        {
            TotalMatchesPlayed = dbContext.Matches.Count(),
            MatchesPlayedThisWeek = await dbContext.Matches.CountAsync(match => match.DatePlayed >= DateTime.Now.AddDays(-7)
                             && match.DatePlayed <= DateTime.Now)
        };

        return dashboardStats;
    }
}
