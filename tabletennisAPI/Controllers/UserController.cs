using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.Services.Users;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Controllers
{
    [Authorize]
    [Route("user")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }

        [HttpGet("/user/firstname")]
        public async Task<ActionResult<IEnumerable<UserIdAndNameDto>>> GetUserInfo()
        {
            return Ok(await _userService.GetUsersInfoAsync());
        }
    }
}
