using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Users;
using TableTennisAPI.Util;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Services.Users
{
    public class UserService(IUserRepository userRepository, IPasswordHelper passwordHelper, TokenProvider tokenProvider) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHelper _passwordHelper = passwordHelper;
        private readonly TokenProvider _tokenProvider = tokenProvider;

        public IEnumerable<User> GetUsers()
        {
            return _userRepository.FindAll();
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

            user.Password = _passwordHelper.HashPassword(user, dto.Password);
            return await _userRepository.Save(user);
        }

        public async Task<TokenResponseDto?> LoginAsync(string email, string password)
        {
            if (email.Equals(string.Empty) || password.Equals(string.Empty)) return null;

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user is null) return null;

            if (!_passwordHelper.VerifyPassword(user, user.Password, password)) return null;

            return await CreateTokenResponse(user);;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(User user)
        {
            return new TokenResponseDto
            {
                AccessToken = _tokenProvider.Create(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = _tokenProvider.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.UpdateUser(user);

            return refreshToken;
        }

        private async Task<User?> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            var user = await _userRepository.FindUserByIdAsync(userId);

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

        public async Task<IEnumerable<UserIdAndNameDto>> GetUsersInfoAsync()
        {
            return await _userRepository.GetUsersInfoAsync();
        }
    }
}
