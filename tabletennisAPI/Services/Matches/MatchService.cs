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
            var opponentsByMatch = await GetOpponentsByMatchIdsAsync(userMatches);
            var formattedMatches = new List<MatchInformationDto>();

            foreach (var userMatch  in userMatches)
            {
                var userName = userMatch.User.FirstName ?? string.Empty;
                var oppenentsName = opponentsByMatch[(userMatch.MatchId, userMatch.UserId)];

                var (winnerName, loserName) = userMatch.IsWinner
                    ? (userName, oppenentsName)
                    : (oppenentsName, userName);

                formattedMatches.Add(new()
                {
                    WinnerName = winnerName,
                    WinnerElo = userMatch.User.Elo,
                    DatePlayed = userMatch.Match.DatePlayed,
                    Winner = userMatch.IsWinner,
                    LoserName = loserName,
                    WinnerScore = userMatch.Match.WinnerScore,
                    LoserScore = userMatch.Match.LoserScore
                });
            }

            return new MatchInformationWithTotalMatchesDto() { MatchInformations = formattedMatches, TotalMatches = userMatches.Count()};
        }

        //private UserMatch GetUserMatch(UserMatch userMatch)
        //{
        //    var formattedUserMatch = new MatchInformationDto()
        //    {

        //    };
        //}

        private async Task<Dictionary<(int MatchId, int UserId), string>> GetOpponentsByMatchIdsAsync(IEnumerable<UserMatch> userMatches)
        {
            var allMatchIds = userMatches
                .Select(um => um.MatchId)
                .Distinct()
                .ToList();

            var userMatchesWithUsers = await _userMatchRepository
                .GetUserMatchesByMatchIdAsync(allMatchIds);

            var opponentsByMatch = userMatchesWithUsers
                .GroupBy(um => um.MatchId)
                .SelectMany(g => g.Select(um => new
                {
                    um.MatchId,
                    um.UserId,
                    Opponents = g
                        .Where(other => other.UserId != um.UserId)
                        .Select(other => other.User?.FirstName)
                        .Where(name => !string.IsNullOrEmpty(name))
                }))
                .ToDictionary(
                    x => (x.MatchId, x.UserId),
                    x => string.Join(", ", x.Opponents)
                );

            return opponentsByMatch;
        }

        public Task<Match?> UpdateMatchAsync(MatchSubmissionDto match)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MatchesPerDayDto>> GetFormattedMatchesByDateAsync(int pageIndex, int pageSize)
        {
            var userMatches = await _userMatchRepository.GetUserMatchesPaginatedAsync(pageIndex, pageSize);
            userMatches = userMatches.Reverse();
            var opponentsByMatch = await GetOpponentsByMatchIdsAsync(userMatches);
            var formattedMatches = new List<MatchesPerDayDto>();
            var groupedUsermatches = userMatches
                .GroupBy(um => um.Match.DatePlayed)
                .ToList();

            foreach (var groupedUsermatch in groupedUsermatches)
            {
                var userMatchInfo = new MatchesPerDayDto() { Date = groupedUsermatch.Key };
                foreach (var match in groupedUsermatch)
                {
                    var userName = match.User.FirstName ?? string.Empty;
                    var oppenentsName = opponentsByMatch[(match.MatchId, match.UserId)];

                    var (winnerName, loserName) = match.IsWinner
                        ? (userName, oppenentsName)
                        : (oppenentsName, userName);

                    userMatchInfo.Matches.Add(new() 
                    {
                        WinnerName = winnerName,
                        WinnerElo = match.User.Elo,
                        DatePlayed = match.Match.DatePlayed,
                        Winner = match.IsWinner,
                        LoserName = loserName,
                        WinnerScore = match.Match.WinnerScore,
                        LoserScore = match.Match.LoserScore
                    });
                }
                formattedMatches.Add(userMatchInfo);
            }
            return formattedMatches;
        }

        public async Task<Match?> GetMatchById(int matchId) => await _matchRepository.FindMatchById(matchId);
    }
}
