using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.UserMatches
{
    public interface IUserMatchRepository
    {
        Task<UserMatch> AddUserMatch(UserMatch userMatch);
        Task<IEnumerable<UserMatch>> GetUserMatchesAsync();
        Task<IEnumerable<UserMatch>> GetUserMatchesByMatchIdAsync(int matchId);
    }
}
