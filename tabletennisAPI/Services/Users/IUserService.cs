using TableTennisAPI.Models;
using TableTennisShared.DTO.User;
using TableTennisShared.DTO.Token;

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
