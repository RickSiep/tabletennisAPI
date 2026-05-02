using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Users;
using TableTennisAPI.Util;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Services.Auth
{
    public class AuthService(IUserRepository userRepository, IPasswordHelper passwordHelper, TokenProvider tokenProvider) : IAuthService
    {
        public async Task<User?> LoginAsync(string email, string password)
        {
            if (email.Equals(string.Empty) || password.Equals(string.Empty))
            {
                return null;
            }

            var user = await userRepository.GetUserByEmailAsync(email);
            if (user is null)
            {
                return null;
            }

            if (!passwordHelper.VerifyPassword(user, user.Password, password))
            {
                return null;
            }

            return user;
        }

        public async Task<User> SaveUserAsync(RegisterDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Roles = "User",
                Elo = 1000
            };

            user.Password = passwordHelper.HashPassword(user, dto.Password);
            return await userRepository.Save(user);
        }

        public async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = tokenProvider.Create(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = tokenProvider.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await userRepository.UpdateUser(user);
            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await userRepository.FindUserByIdAsync(userId);

            if (user is null || user.RefreshToken != refreshToken
                || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return null;
            }

            return user;

        }

        public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

            if (user is null)
                return null;

            return await CreateTokenResponse(user);
        }

        public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await userRepository.GetUserByRefreshTokenAsync(refreshToken);
        }
    }
}
