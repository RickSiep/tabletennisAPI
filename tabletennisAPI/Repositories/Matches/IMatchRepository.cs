using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.Matches
{
    public interface IMatchRepository
    {
        Task<Match> AddMatchAsync(Match match);
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        IEnumerable<UserMatch> GetUserMatches();

    }
}
