using TableTennisAPI.Models;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<User?> LoginAsync(string email, string password);
        Task<User> SaveUserAsync(RegisterDto dto);
        Task<string> GenerateAndSaveRefreshTokenAsync(User user);
        Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<TokenResponseDto> CreateTokenResponse(User user);
    }
}
