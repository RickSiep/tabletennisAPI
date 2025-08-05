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

        public async Task<Match?> FindMatchById(int id) => await _context.Matches.FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return _context.Matches.ToList();
        }
    }
}
