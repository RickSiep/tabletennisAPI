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
    }
}
