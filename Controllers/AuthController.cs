using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.DTO.Token;
using TableTennisAPI.DTO.User;
using TableTennisAPI.Models;
using TableTennisAPI.Services.Users;
using TableTennisAPI.Util;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        public async Task<IActionResult> Login(string email, string password)
        {
            var jwt = await _userService.LoginAsync(email, password);

            if (jwt is null) return BadRequest("Login failed");

            return Ok(jwt);
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
