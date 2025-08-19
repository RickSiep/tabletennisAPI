using Microsoft.EntityFrameworkCore;
using TableTennisAPI.Data;
using TableTennisAPI.Models;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Repositories.Users
{
    public class UserRepository(DatabaseContext context) : IUserRepository
    {

        private readonly DatabaseContext _context = context;

        public List<User> FindAll()
        {
            return _context.Users.ToList();
        }

        public async Task<User?> FindUserByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<UserIdAndNameDto>> GetUsersInfoAsync()
        {
            return await _context.Users
                .Select(u => new UserIdAndNameDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName
                })
                .ToListAsync();
        }

        public async Task<User> Save(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            
            await _context.SaveChangesAsync();

            return;
        }
    }
}
