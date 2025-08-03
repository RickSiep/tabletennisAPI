using TableTennisAPI.Data;
using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.Matches
{
    public class MatchRepository(DatabaseContext context) : IMatchRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<Match> AddMatch(Match match)
        {
            await _context.Matches.AddAsync(match);

            await _context.SaveChangesAsync();

            return match;
        }

        public async Task<IEnumerable<Match>> GetAllMatches()
        {
            return _context.Matches.ToList();
        }
    }
}
