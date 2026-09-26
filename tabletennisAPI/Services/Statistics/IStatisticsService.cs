using TableTennisShared.DTO.Dashboard;

namespace TableTennisAPI.Services.Statistics;

public interface IStatisticsService
{
    public Task<DashboardStatsDto> GetDashboardStatsAsync();
}
