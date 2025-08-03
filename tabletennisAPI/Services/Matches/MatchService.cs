using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Matches;
using TableTennisAPI.Repositories.UserMatches;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Services.Matches
{
    public class MatchService(IMatchRepository matchRepository, IUserMatchRepository userMatchRepository) : IMatchService
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly IUserMatchRepository _userMatchRepository = userMatchRepository;

        public async Task<Match?> SaveMatchAsync(MatchSubmissionDto match)
        {
            var newMatch = await _matchRepository.AddMatch(new() { DatePlayed = DateTime.Today.Date});

            foreach (var participant in match.Participants)
            {
                var userMatch = new UserMatch();
                userMatch.MatchId = newMatch.Id;
                userMatch.UserId = participant.UserId;
                userMatch.IsWinner = participant.IsWinner;
                userMatch.TeamNumber = participant.TeamNumber;

                await _userMatchRepository.AddUserMatch(userMatch);
            }

            return newMatch;
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync() => await _matchRepository.GetAllMatches();
    }
}
