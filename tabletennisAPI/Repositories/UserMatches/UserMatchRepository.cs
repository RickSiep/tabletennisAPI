using Microsoft.EntityFrameworkCore;
using TableTennisAPI.Data;
using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.UserMatches
{
    public class UserMatchRepository(DatabaseContext context) : IUserMatchRepository
    {
        private readonly DatabaseContext _context = context;

        public async Task<UserMatch> AddUserMatch(UserMatch userMatch)
        {
            await _context.UserMatches.AddAsync(userMatch);
            await _context.SaveChangesAsync();
            return userMatch;
        }

        public async Task<IEnumerable<UserMatch>> GetUserMatchesAsync()
        {
            return await _context.UserMatches
                .Include(um => um.User)
                .Include(um => um.Match)
                .ToListAsync();
        }

        public async Task<int> GetTotalAmountOfMatches()
        {
            return await _context.UserMatches.CountAsync();
        }

        public async Task<IEnumerable<UserMatch>> GetUserMatchesPaginatedAsync(int pageIndex = 1, int pageSize = 10)
        {
            var matches = await _context.UserMatches
                .Include(um => um.User)
                .Include(um => um.Match)
                .OrderBy(um => um.Match.DatePlayed)
                .GroupBy(x => x.MatchId)
                .Select(x => x.FirstOrDefault())
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return matches;
        }

        public async Task<IEnumerable<UserMatch>> GetUserMatchesByMatchIdsAsync(IEnumerable<int> matchIds)
        {
            return await _context.UserMatches
                .Include(um => um.User)
                .Where(um => matchIds.Contains(um.MatchId))
                .ToListAsync();
        }

        public async Task<List<UserMatch>> GetUserMatchesByMatchIdsAsync(int matchId) => 
                await _context.UserMatches
                .Include(usermatch => usermatch.User)
                .Where(usermatch => usermatch.MatchId == matchId)
                .ToListAsync();
    }
}
