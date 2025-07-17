using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.Services.Users;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Controllers
{
    [Route("user")]
    [ApiController]
    public class AuthController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        // POST api/<RegisterController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _userService.SaveUserAsync(dto);

            return Ok("User Registered");
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _userService.GetUsers();
            return Ok(users);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var jwt = await _userService.LoginAsync(dto.Email, dto.Password);

            return jwt is null ? BadRequest("Username password combination isn't known.") : Ok(jwt);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _userService.RefreshTokenAsync(request);

            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token");

            return Ok(result);
        }


        //[Authorize]
        //[HttpGet("test")]
        //public IActionResult AuthenticatedEndpoint()
        //{
        //    return Ok("What an user!");
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet("testAdmin")]
        //public IActionResult AuthenticatedAdmin()
        //{
        //    return Ok("What an admin!");
        //}
    }
}
