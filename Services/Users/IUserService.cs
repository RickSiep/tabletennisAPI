using TableTennisAPI.DTO.User;
using TableTennisAPI.Models;

namespace TableTennisAPI.Services.Users {
    public interface IUserService {
        IEnumerable<User> GetUsers();
        Task<User> SaveUserAsync(RegisterDTO dto);

        Task<User?> LoginAsync(string email, string password);
    }
}
