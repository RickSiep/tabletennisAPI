using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.Services.Auth;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        // POST api/<RegisterController>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await authService.SaveUserAsync(dto);

            return Ok("User Registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var jwt = await authService.LoginAsync(dto.Email, dto.Password);

            return jwt is null ? BadRequest("Username password combination isn't known.") : Ok(jwt);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);

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
