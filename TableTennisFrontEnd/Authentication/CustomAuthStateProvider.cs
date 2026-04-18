using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace TableTennisFrontEnd.Authentication
{
    public class CustomAuthStateProvider(ProtectedLocalStorage storage) : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _storage = storage;
        private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = (await _storage.GetAsync<string>("authToken")).Value;
                var identity = string.IsNullOrEmpty(token) ? _anonymous : new ClaimsPrincipal(GetClaimsIdentity(token));
                return new AuthenticationState(identity);
            }
            catch (Exception)
            {
                return new AuthenticationState(_anonymous);
            }
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var json = Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(payload)));
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            if (keyValuePairs != null)
            {
                var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
                return claims;
            }

            return null;
        }

        public async Task NotifyUserAuthentication(string accessToken, string refreshToken)
        {
            await _storage.SetAsync("authToken", accessToken);
            await _storage.SetAsync("refreshToken", refreshToken);

            var identity = GetClaimsIdentity(accessToken);
            var user = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            var claims = ParseClaimsFromJwt(token);
            return new ClaimsIdentity(claims, "jwt");
        }

        public static string PadBase64(string base64) => (base64.Length % 4) switch
        {
            2 => base64 + "==",
            3 => base64 + "=",
            _ => base64
        };

        public async Task Logout()
        {
            await _storage.DeleteAsync("authToken");
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
