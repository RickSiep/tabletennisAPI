using TableTennisAPI.Models;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Services.Users
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        Task<IEnumerable<UserIdAndNameDto>> GetUsersInfoAsync();
    }
}
