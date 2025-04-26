using Microsoft.AspNetCore.Identity;
using TableTennisAPI.Models;

namespace TableTennisAPI.Util {
    public class PasswordHelper : IPasswordHelper {
        private readonly IPasswordHasher<User> _passwordHasher;

        public PasswordHelper(IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
        }

        public string HashPassword(User user, string password)
        {
            return _passwordHasher.HashPassword(user, password);
        }

        public bool VerifyPassword(User user, string hashedPassword, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);

            return result == PasswordVerificationResult.Success;
        }
    }
}
