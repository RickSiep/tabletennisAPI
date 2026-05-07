using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TableTennisShared.DTO.Token;

namespace TableTennisFrontEnd.Pages.Account
{
    public class RefreshTokenModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync([FromBody] TokenResponseDto tokens)
        {
            var user = HttpContext.User;
            if (user.Identity?.IsAuthenticated != true)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>()
            {
                new(ClaimTypes.Name, user.FindFirst(ClaimTypes.Name)!.Value),
                new(ClaimTypes.NameIdentifier, user.FindFirst(ClaimTypes.NameIdentifier)!.Value),
                new("access_token", tokens.AccessToken),
                new("refresh_token", tokens.RefreshToken)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("Cookies", principal);
            return new OkResult();
        }
    }
}
