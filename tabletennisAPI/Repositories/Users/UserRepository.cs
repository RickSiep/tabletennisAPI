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

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<LocalCredential?> GetLocalCredentialByUserIdAsync(int id) => await _context.LocalCredentials.FirstOrDefaultAsync(lc => lc.Id == id);

        public async Task<LocalCredential?> SaveLocalUserCredential(LocalCredential localCredential)
        {
            _context.LocalCredentials.Add(localCredential);

            await _context.SaveChangesAsync();

            return localCredential;
        }

        public async Task<ExternalCredential?> GetExternalCredentialByProviderIdAsync(string providerUserId) 
            => await _context.ExternalCredentials.FirstOrDefaultAsync(ec => ec.ProviderUserId == providerUserId);

        public async Task<ExternalCredential> SaveExternalUserCredential(ExternalCredential externalCredential)
        {
            _context.ExternalCredentials.Add(externalCredential);

            await _context.SaveChangesAsync();

            return externalCredential;
        }

        public async Task<User?> GetUserByExternalCredentialAsync(ExternalCredential externalCredential) 
            => await _context.Users.FirstOrDefaultAsync(user => user.Id == externalCredential.Id);
        
    }
}
