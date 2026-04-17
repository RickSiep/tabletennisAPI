using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.UserMatches
{
    public interface IUserMatchRepository
    {
        Task<UserMatch> AddUserMatch(UserMatch userMatch);
        Task<int> GetTotalAmountOfMatches();
        Task<IEnumerable<UserMatch>> GetUserMatchesAsync();
        Task<IEnumerable<UserMatch>> GetUserMatchesPaginatedAsync(int pageIndex, int pageSize);
        Task<IEnumerable<UserMatch>> GetUserMatchesByMatchIdsAsync(IEnumerable<int> matchIds);
        Task DeleteUserMatchesByMatchIdAsync(int matchId);
        Task<List<UserMatch>> GetUserMatchesByMatchIdsAsync(int matchId);
    }
}
