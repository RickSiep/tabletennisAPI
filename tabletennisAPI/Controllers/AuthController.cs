using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
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
            var user = await authService.LoginAsync(dto.Email, dto.Password);

            if (user == null)
            {
                return BadRequest("Email password combination isn't known.");
            }

            var tokens = await authService.CreateTokenResponse(user);

            return Ok(new UserInfoWithTokens() 
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await authService.RefreshTokenAsync(request);

            if (result is null || result.AccessToken is null || result.RefreshToken is null)
                return Unauthorized("Invalid refresh token");

            return Ok(result);
        }

        [HttpPost("refresh-token-from-cookie")]
        public async Task<ActionResult> RefreshTokenFromCookie([FromBody]string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var user = await authService.GetUserByRefreshTokenAsync(refreshToken);
            var tokens = await authService.RefreshTokenAsync(new RefreshTokenRequestDto
            {
                UserId = user.Id,
                RefreshToken = refreshToken
            });

            return Ok(tokens);
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
