using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.Matches
{
    public interface IMatchRepository
    {
        Task<Match> AddMatchAsync(Match match);
        Task<Match> UpdateMatchAsync(Match match);
        Task DeleteMatchAsync(int matchId);
        Task<Match?> FindMatchById(int id);
        Task<IEnumerable<Match>> GetAllMatchesAsync();
    }
}
