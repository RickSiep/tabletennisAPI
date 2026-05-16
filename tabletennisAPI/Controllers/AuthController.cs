using Microsoft.AspNetCore.Mvc;
using TableTennisAPI.Models;
using TableTennisAPI.Services.Auth;
using TableTennisAPI.Services.Users;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisAPI.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(IAuthService authService, IUserService userService) : ControllerBase
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

            var tokens = await authService.CreateTokenResponseAsync(user);

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
        public async Task<ActionResult> RefreshTokenFromCookie([FromBody] string refreshToken)
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

        [HttpPost("get-access-token-from-refresh")]
        public async Task<ActionResult<string>> GetAccessTokenFromRefresh([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var user = await authService.GetUserByRefreshTokenAsync(refreshToken);
            if (user == null)
            {
                return Unauthorized();
            }

            var accessToken = authService.CreateAccessToken(user);

            return string.IsNullOrEmpty(accessToken) ? Unauthorized() : Ok(accessToken);
        }

        [HttpPost("external-login")]
        public async Task<ActionResult> ExternalLogin([FromBody] ExternalUserRegisterDto externalUserRegister)
        {
            var externalCred = await authService.GetExternalCredentialByProviderUserIdAsync(externalUserRegister.ProviderUserId);

            if (externalCred == null)
            {
                var user = await authService.RegisterExternalCredentialUser(externalUserRegister);
                var tokens = await authService.CreateTokenResponseAsync(user);

                return Ok(new UserInfoWithTokens()
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken,
                });
            }

            var newUser = await authService.GetUserByExternalCred(externalCred);
            if (newUser == null)
            {
                return BadRequest("Couldn't find user");
            }
            var newTokens = await authService.CreateTokenResponseAsync(newUser);

            return Ok(new UserInfoWithTokens()
                {
                    UserId = newUser.Id,
                    FirstName = newUser.FirstName,
                    AccessToken = newTokens.AccessToken,
                    RefreshToken = newTokens.RefreshToken,
                });
        }

        //[Authorize]
        //[HttpGet("test")]
        //public IActionResult AuthenticatedEndpoint()
        //{
        //    return Ok("user!");
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpGet("testAdmin")]
        //public IActionResult AuthenticatedAdmin()
        //{
        //    return Ok("admin!");
        //}
    }
}
