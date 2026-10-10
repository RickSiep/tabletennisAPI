using Microsoft.EntityFrameworkCore;
using TableTennisAPI.Data;
using TableTennisShared.DTO.Dashboard;

namespace TableTennisAPI.Repositories.Statistics;

public class StatisticsRepository(DatabaseContext dbContext) : IStatisticsRepository
{
    public async Task<DashboardStatsDto> GetDashboardStatsAsync(int userId)
    {
        var dashboardStats = new DashboardStatsDto
        {
            TotalMatchesPlayed = dbContext.Matches.Count(),
            MatchesPlayedThisWeek = await dbContext.Matches.CountAsync(match => match.DatePlayed >= DateTime.Now.AddDays(-7)
                             && match.DatePlayed <= DateTime.Now),
            WinRate = await GetUserWinRateAsync(userId)
        };

        return dashboardStats;
    }

    private async Task<double> GetUserWinRateAsync(int userId)
    {
        // Krijg eerst alle wins
        var winnerMatches = await dbContext.UserMatches.CountAsync(user => user.UserId == userId && user.IsWinner).ConfigureAwait(false);
        var allMatches = await dbContext.UserMatches.CountAsync(user => user.UserId == userId).ConfigureAwait(false);
        return ((double)winnerMatches / (double)allMatches) * 100;
    }
}
