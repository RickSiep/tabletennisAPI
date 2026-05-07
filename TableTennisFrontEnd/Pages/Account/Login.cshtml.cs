using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using TableTennisFrontEnd.Authentication;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisFrontEnd.Pages.Account
{
    public class LoginModel(IHttpClientFactory factory, AuthState authState) : PageModel
    {
        [BindProperty] public string Email { get; set; } = string.Empty;
        [BindProperty] public string Password { get; set; } = string.Empty;
        [BindProperty] public string ReturnUrl { get; set; } = string.Empty;

        public IActionResult OnGet(string returnUrl = "/")
        {
            ReturnUrl = returnUrl;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // call your external API to validate credentials
            var http = factory.CreateClient("api");
            var response = await http.PostAsJsonAsync("auth/login", new LoginRequestDto { Email = Email, Password = Password });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return Page();
            }

            var result = await response.Content.ReadFromJsonAsync<UserInfoWithTokens>();
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Response couldn't be read");
                return Page();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, result.FirstName),
                new(ClaimTypes.NameIdentifier, result.UserId.ToString()),
                new("refresh_token", result.RefreshToken)
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            authState.RefreshToken = result.RefreshToken;

            await HttpContext.SignInAsync("Cookies", principal); // ✅ THIS is the key

            return LocalRedirect(ReturnUrl == string.Empty ? "/" : ReturnUrl);
        }
    }
}
