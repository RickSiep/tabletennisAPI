using TableTennisAPI.Models;

namespace TableTennisAPI.Util {
    public interface IPasswordHelper {
        string HashPassword(User user, string password);
        bool VerifyPassword(User user, string hashedPassword, string password);
    }
}
