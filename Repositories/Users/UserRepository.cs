using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TableTennisAPI.Data;
using TableTennisAPI.DTO.User;
using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.Users
{
    public class UserRepository(DatabaseContext context) : IUserRepository
    {

        private readonly DatabaseContext _context = context;

        public List<User> FindAll()
        {
            return _context.Users.ToList();
        }

        public async Task<User?> FindUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
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
