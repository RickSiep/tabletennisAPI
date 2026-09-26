using TableTennisAPI.Repositories.Statistics;
using TableTennisShared.DTO.Dashboard;

namespace TableTennisAPI.Services.Statistics;

public class StatisticsService(IStatisticsRepository statisticsRepository) : IStatisticsService
{
    public async Task<DashboardStatsDto> GetDashboardStatsAsync() => await statisticsRepository.GetDashboardStatsAsync().ConfigureAwait(false);
}
