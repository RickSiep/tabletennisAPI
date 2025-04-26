using TableTennisAPI.Models;

namespace TableTennisAPI.Services.Matches
{
    public interface IMatchService
    {
        Task<Match?> SaveMatchAsync(int winnerId, int loserId, int winnerScore, int loserScore);
    }
}
