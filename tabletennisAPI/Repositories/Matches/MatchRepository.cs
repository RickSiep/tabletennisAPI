using Microsoft.EntityFrameworkCore;
using TableTennisAPI.Data;
using TableTennisAPI.Models;
using TableTennisShared.DTO.Match;

namespace TableTennisAPI.Repositories.Matches
{
    public class MatchRepository(DatabaseContext context) : IMatchRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<Match> AddMatchAsync(Match match)
        {
            await _context.Matches.AddAsync(match);

            await _context.SaveChangesAsync();

            return match;
        }

        public async Task<Match> UpdateMatchAsync(Match match)
        {
            _context.Matches.Update(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task<Match?> FindMatchById(int id) => await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<Match>> GetAllMatchesAsync() => await _context.Matches.ToListAsync();

        public async Task DeleteMatchAsync(int matchId)
        {
            var match = await FindMatchById(matchId);
            if (match is not null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
            }
        }
    }
}
