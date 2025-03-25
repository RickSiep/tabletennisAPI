using TableTennisAPI.DTO.Token;
using TableTennisAPI.DTO.User;
using TableTennisAPI.Models;

namespace TableTennisAPI.Services.Users
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        Task<User> SaveUserAsync(RegisterDto dto);

        Task<TokenResponseDto?> LoginAsync(string email, string password);

        Task<string> GenerateAndSaveRefreshTokenAsync(User user);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

    }
}
