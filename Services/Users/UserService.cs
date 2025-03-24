using TableTennisAPI.DTO.User;
using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Users;
using TableTennisAPI.Util;

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

        public async Task<User> SaveUserAsync(RegisterDTO dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                IsAdmin = false,
                Elo = 1000
            };

            user.Password = _passwordHelper.HashPassword(user, dto.Password);
            return await _userRepository.Save(user);
        }

        public async Task<string?> LoginAsync(string email, string password)
        {
            if (email.Equals(string.Empty) || password.Equals(string.Empty)) return null;

            var user = await _userRepository.GetUserByEmailAsync(email);

            if (user is null) return null;

            return _passwordHelper.VerifyPassword(user, user.Password, password) ? _tokenProvider.Create(user) : null;
        }

    }
}
