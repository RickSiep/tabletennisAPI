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
            var newMatch = await _matchRepository.AddMatchAsync(new() { DatePlayed = DateTime.Today.Date, WinnerScore = match.WinnerScore, LoserScore = match.LoserScore});

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

        public async Task<MatchInformationWithTotalMatchesDto> GetFormattedMatchesAsync(int pageIndex, int pageSize)
        {
            var userMatches = await _userMatchRepository.GetUserMatchesPaginatedAsync(pageIndex, pageSize);
            var formattedMatches = new List<MatchInformationDto>();

            foreach (var userMatch  in userMatches)
            {
                formattedMatches.Add(new() 
                { 
                    FirstName = userMatch.User.FirstName ?? string.Empty, 
                    Elo = userMatch.User.Elo, 
                    DatePlayed = userMatch.Match.DatePlayed,
                    Winner = userMatch.IsWinner,
                    PlayedAgainst = await GetPlayedAgainstUsernames(userMatch.MatchId, userMatch.UserId)
                });
            }

            return new MatchInformationWithTotalMatchesDto() { MatchInformations = formattedMatches, TotalMatches = userMatches.Count()};
        }

        private async Task<string> GetPlayedAgainstUsernames(int matchId, int userId)
        {
            var userMatches = await _userMatchRepository.GetUserMatchesByMatchIdAsync(matchId);
            var userNameString = string.Empty;

            foreach (var userMatch in userMatches)
            {
                var user = await _userRepository.FindUserByIdAsync(userMatch.UserId);
                if (user != null && user.Id != userId)
                {
                    userNameString += $"{user.FirstName}, ";
                }
            }

            return userNameString.TrimEnd([',', ' ']);
        }

        public Task<Match?> UpdateMatchAsync(MatchSubmissionDto match)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MatchesPerDayDto>> GetFormattedMatchesByDateAsync(int pageIndex, int pageSize)
        {
            var userMatches = await _userMatchRepository.GetUserMatchesPaginatedAsync(pageIndex, pageSize);
            var formattedMatches = new List<MatchesPerDayDto>();
            var groupedUsermatches = userMatches
                .GroupBy(um => um.Match.DatePlayed)
                .ToList();

            foreach (var groupedUsermatch in groupedUsermatches)
            {
                var userMatchInfo = new MatchesPerDayDto() { Date = groupedUsermatch.Key };
                foreach (var match in groupedUsermatch)
                {
                    Console.WriteLine(match.UserId);
                }
                formattedMatches.Add(new()
                {
                    Date = groupedUsermatch.Key
                    //Matches = groupedUsermatch.Value
                });
            }

            return new();
        }
    }
}
