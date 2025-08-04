using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Matches;
using TableTennisAPI.Repositories.UserMatches;
using TableTennisAPI.Repositories.Users;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Services.Matches
{
    public class MatchService(IMatchRepository matchRepository, IUserMatchRepository userMatchRepository, IUserRepository userRepository) : IMatchService
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly IUserMatchRepository _userMatchRepository = userMatchRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Match?> SaveMatchAsync(MatchSubmissionDto match)
        {
            var newMatch = await _matchRepository.AddMatchAsync(new() { DatePlayed = DateTime.Today.Date});

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

        public async Task<IEnumerable<Match>> GetAllMatchesAsync() => await _matchRepository.GetAllMatchesAsync();

        public async Task<IEnumerable<MatchInformationDto>> GetFormattedMatchesAsync()
        {
            var userMatches = _matchRepository.GetUserMatches();
            var formattedMatches = new List<MatchInformationDto>();

            foreach (var userMatch  in userMatches)
            {
                var user = await _userRepository.FindUserByIdAsync(userMatch.UserId);
                var match = await _matchRepository.FindMatchById(userMatch.MatchId);
                var firstName = user.FirstName ?? string.Empty;
                formattedMatches.Add(new() 
                { 
                    FirstName = user.FirstName ?? string.Empty, 
                    Elo = (int)user.Elo, 
                    DatePlayed = match.DatePlayed,
                    Winner = (bool)userMatch.IsWinner 
                });
                //matchInformation.FirstName = 
            }

            return formattedMatches;
        }
    }
}
