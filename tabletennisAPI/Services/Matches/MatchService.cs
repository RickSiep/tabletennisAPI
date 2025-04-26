using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Matches;
using TableTennisAPI.Repositories.Users;

namespace TableTennisAPI.Services.Matches
{
    public class MatchService(IMatchRepository matchRepository, IUserRepository userRepository) : IMatchService
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly IUserRepository _userRepository = userRepository;

        public async Task<Match?> SaveMatchAsync(int winnerId, int loserId, int winnerScore, int loserScore)
        {
            var winner = await _userRepository.FindUserById(winnerId);
            var loser = await _userRepository.FindUserById(loserId);

            if (winner is null || loser is null) return null;

            var match = new Match
            {
                MatchWinner = winner,
                MatchLoser = loser,
                WinnerScore = winnerScore,
                LoserScore = loserScore
            };

            return await _matchRepository.AddMatch(match);
        }
    }
}
