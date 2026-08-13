using TableTennisAPI.Models;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Services.Matches
{
    public interface IMatchService
    {
        Task<Match?> SaveMatchAsync(MatchSubmissionDto match);
        Task<Match?> SaveMatchAsync(SinglesMatchDto singlesMatch);
        Task<Match?> UpdateMatchAsync(MatchSubmissionDto match);
        Task DeleteMatchAsync(int matchId);
        Task<Match?> GetMatchById(int matchId);
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<List<MatchesPerDayDto>> GetFormattedMatchesByDateAsync(int pageIndex, int pageSize);

        Task<MatchInformationWithTotalMatchesDto> GetFormattedMatchesAsync(int pageIndex, int pageSize);
    }
}
