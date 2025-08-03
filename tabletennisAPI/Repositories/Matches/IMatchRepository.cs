using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.Matches
{
    public interface IMatchRepository
    {
        Task<Match> AddMatch(Match match);
        Task<IEnumerable<Match>> GetAllMatches();

    }
}
