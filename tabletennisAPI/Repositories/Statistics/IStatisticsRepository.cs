using TableTennisShared.DTO.Dashboard;

namespace TableTennisAPI.Repositories.Statistics;

public interface IStatisticsRepository
{
    Task<DashboardStatsDto> GetDashboardStatsAsync();
}
