using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Matches;
using TableTennisAPI.Repositories.UserMatches;
using TableTennisAPI.Repositories.Users;
using TableTennisAPI.Util;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Services.Matches
{
    public class MatchService(IMatchRepository matchRepository, IUserMatchRepository userMatchRepository, IUserRepository userRepository) : IMatchService
    {

        public async Task<Match?> SaveMatchAsync(MatchSubmissionDto match)
        {
            var newMatch = await matchRepository.AddMatchAsync(new(DateTime.Today.Date, match.WinnerScore, match.LoserScore));

            foreach (var participant in match.Participants)
            {
                var userMatch = new UserMatch
                {
                    MatchId = newMatch.Id,
                    UserId = participant.UserId,
                    IsWinner = participant.IsWinner,
                    TeamNumber = participant.TeamNumber
                };

                await userMatchRepository.AddUserMatch(userMatch);
            }

            return newMatch;
        }

        public async Task<Match?> SaveMatchAsync(SinglesMatchDto singlesMatch)
        {
            if (singlesMatch == null)
            {
                return null;
            }

            var match = await matchRepository.AddMatchAsync(new(DateTime.Today.Date, singlesMatch.WinnerScore, singlesMatch.LoserScore));

            await SaveUserMatch(match.Id, singlesMatch.WinnerId, true);
            await SaveUserMatch(match.Id, singlesMatch.LoserId, false);

            await SetUserEloAfterMatch(singlesMatch.WinnerId, singlesMatch.LoserId);

            return match;
        }

        private async Task SetUserEloAfterMatch(int winnerId, int loserId)
        {
            var winner = await userRepository.GetUserByIdAsync(winnerId);
            var loser = await userRepository.GetUserByIdAsync(loserId);

            var (winnerElo, loserElo) = EloCalculator.CalculateEloForSinglesGame(winner.SinglesRating, loser.SinglesRating);

            winner.SinglesRating = (int)winnerElo;
            loser.SinglesRating = (int)loserElo;

            await userRepository.UpdateUser(winner);
            await userRepository.UpdateUser(loser);
        }

        private Task SaveUserMatch(int matchId, int userId, bool isWinner) 
            => userMatchRepository.AddUserMatch(new() { MatchId = matchId, UserId =  userId, IsWinner = isWinner });
        
        public async Task<IEnumerable<Match>> GetAllMatchesAsync() => await matchRepository.GetAllMatchesAsync();

        public async Task<MatchInformationWithTotalMatchesDto> GetFormattedMatchesAsync(int pageIndex, int pageSize)
        {
            var totalMatches = (await matchRepository.GetAllMatchesAsync()).ToList().Count;
            var userMatches = await userMatchRepository.GetUserMatchesPaginatedAsync(pageIndex, pageSize);
            var opponentsByMatch = await GetOpponentsByMatchIdsAsync(userMatches);
            var formattedMatches = new List<MatchInformationDto>();

            foreach (var userMatch in userMatches)
            {
                var userName = userMatch.User.FirstName ?? string.Empty;
                var oppenentsName = opponentsByMatch[(userMatch.MatchId, userMatch.UserId)];

                var (winnerName, loserName) = userMatch.IsWinner
                    ? (userName, oppenentsName)
                    : (oppenentsName, userName);

                formattedMatches.Add(new()
                {
                    MatchId = userMatch.MatchId,
                    WinnerName = winnerName,
                    WinnerElo = userMatch.User.SinglesRating,
                    DatePlayed = userMatch.Match.DatePlayed,
                    Winner = userMatch.IsWinner,
                    LoserName = loserName,
                    WinnerScore = userMatch.Match.WinnerScore,
                    LoserScore = userMatch.Match.LoserScore
                });
            }

            return new() { MatchInformations = formattedMatches, TotalMatches = totalMatches };
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

            var userMatchesWithUsers = await userMatchRepository
                .GetUserMatchesByMatchIdsAsync(allMatchIds);

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

        public async Task<Match?> UpdateMatchAsync(MatchSubmissionDto match)
        {
            //var updatedMatch = await _matchRepository.UpdateMatchAsync(match);
            throw new NotImplementedException();
        }

        public async Task<List<MatchesPerDayDto>> GetFormattedMatchesByDateAsync(int pageIndex, int pageSize)
        {
            var userMatches = await userMatchRepository.GetUserMatchesPaginatedAsync(pageIndex, pageSize);
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
                        WinnerElo = match.User.SinglesRating,
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

        public async Task<Match?> GetMatchById(int matchId) => await matchRepository.FindMatchById(matchId);

        public async Task DeleteMatchAsync(int matchId) => await matchRepository.DeleteMatchAsync(matchId);
    }
}
