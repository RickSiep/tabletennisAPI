using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using System.Threading;
using TableTennisShared.DTO.JWT;
using TableTennisShared.DTO.Token;
using TableTennisShared.DTO.User;

namespace TableTennisFrontEnd.Authentication
{
    public class AuthService(IHttpContextAccessor httpContextAccessor, AuthState authState, IHttpClientFactory factory)
    {
        //public async Task InitializeAsync()
        //{
        //    var context = httpContextAccessor.HttpContext;

        //    var refreshToken = context?.Request.Cookies["refreshToken"];
        //    if (refreshToken == null)
        //    {
        //        return;
        //    }

        //    var newTokens = await RefreshTokensAsync(refreshToken);
        //    if (newTokens == null)
        //    {
        //        return;
        //    }

        //    authState.SetTokens(newTokens.AccessToken, newTokens.RefreshToken);
        //}

        //private async Task<TokenResponseDto?> RefreshTokensAsync(string refreshToken)
        //{
        //    try
        //    {
        //        var apiClient = new ApiClient(factory.CreateClient());
        //        var response = await apiClient.PostJsonAsync("/auth/refresh-token-from-cookie", refreshToken);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return null;
        //        }

        //        return await response.Content.ReadFromJsonAsync<TokenResponseDto>();
        //    }
        //    catch
        //    {

        //    }

        //    return null;
        //}

        public async Task CreateClaimsAndSignIn()
        {

        }

        public async Task<bool> SignInAsync(ApiClient apiClient, LoginRequestDto loginRequest)
        {
            var response = await apiClient.PostJsonAsyncWithResponseModel<LoginRequestDto, UserInfoWithTokens>("/auth/login", loginRequest);

            if (response == null)
            {
                return false;
            }

            authState.SetTokens(response.AccessToken, response.RefreshToken);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, response.FirstName),
                new(ClaimTypes.NameIdentifier, response.UserId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            var context = httpContextAccessor.HttpContext;
            await httpContextAccessor.HttpContext!.SignInAsync("Cookies", principal);

            return true;
        }

        public async Task Logout()
        {
            authState.Clear();

            await httpContextAccessor.HttpContext!.SignOutAsync("Cookies");
        }
    }
}
