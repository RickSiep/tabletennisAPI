using TableTennisAPI.Models;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Repositories.Users {
    public interface IUserRepository {
        List<User> FindAll();
        Task<User?> FindUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<LocalCredential?> GetLocalCredentialByUserIdAsync(Guid id);
        Task<IEnumerable<UserIdAndNameDto>> GetUsersInfoAsync();
        Task<User> Save(User user);
        Task UpdateUser(User user);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
    }
}
