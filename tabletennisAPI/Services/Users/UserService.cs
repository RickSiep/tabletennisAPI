using TableTennisAPI.Models;
using TableTennisAPI.Repositories.Users;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Services.Users
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public IEnumerable<User> GetUsers()
        {
            return userRepository.FindAll();
        }

        public async Task<IEnumerable<UserIdAndNameDto>> GetUsersInfoAsync()
        {
            return await userRepository.GetUsersInfoAsync();
        }
    }
}
