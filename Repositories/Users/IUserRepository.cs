using TableTennisAPI.DTO.Token;
using TableTennisAPI.DTO.User;
using TableTennisAPI.Models;

namespace TableTennisAPI.Repositories.Users {
    public interface IUserRepository {
        List<User> FindAll();
        Task<User?> FindUserById(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> Save(User user);
        Task UpdateUser(User user);
    }
}
