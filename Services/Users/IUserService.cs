using TableTennisAPI.DTO.User;
using TableTennisAPI.Models;

namespace TableTennisAPI.Services.Users {
    public interface IUserService {
        IEnumerable<User> GetUsers();
        Task<User> SaveUserAsync(RegisterDTO dto);

        Task<string?> LoginAsync(string email, string password);
    }
}
