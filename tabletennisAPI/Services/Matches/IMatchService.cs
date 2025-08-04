using TableTennisAPI.Models;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Services.Matches
{
    public interface IMatchService
    {
        Task<Match?> SaveMatchAsync(MatchSubmissionDto match);
        Task<IEnumerable<Match>> GetAllMatchesAsync();

        Task<IEnumerable<MatchInformationDto>> GetFormattedMatchesAsync();
    }
}
