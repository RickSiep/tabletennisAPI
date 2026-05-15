using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
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

        public IActionResult OnGetGoogleLogin(string returnUrl = "/")
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Page("/Account/Login", pageHandler: "GoogleCallback", values: new { returnUrl }, protocol: Request.Scheme)
            };
            return new ChallengeResult(GoogleDefaults.AuthenticationScheme, properties);
        }

        public async Task<IActionResult> OnGetGoogleCallbackAsync(string returnUrl = "/")
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return RedirectToPage("/Account/Login");
            }

            var claims = authenticateResult.Principal.Claims.ToList();
            var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var nameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (emailClaim == null || nameClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            // maybe different function
            var http = factory.CreateClient("api");
            var response = await http.PostAsJsonAsync("auth/external-login", new ExternalUserRegisterDto
            {
                Email = emailClaim.Value,
                Name = nameClaim.Value,
                Provider = "Google",
                ProviderUserId = "wehweh" // debug what claim the provider is in
            });

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("Cookies", principal);
            return LocalRedirect(returnUrl);
        }
    }
}
