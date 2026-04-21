using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace TableTennisFrontEnd.Authentication
{
    public class CustomAuthStateProvider(TokenStorageHandler tokenStorage) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (!tokenStorage.IsInitialized)
            {
                return Task.FromResult(new AuthenticationState(_anonymous));
            }

            if (string.IsNullOrEmpty(tokenStorage.AccessToken))
            {
                return Task.FromResult(new AuthenticationState(_anonymous));
            }

            try
            {
                var token = tokenStorage.AccessToken;
                var identity = string.IsNullOrEmpty(token) ? _anonymous : new ClaimsPrincipal(GetClaimsIdentity(token));
                return Task.FromResult(new AuthenticationState(identity));
            }
            catch (Exception)
            {
                return Task.FromResult(new AuthenticationState(_anonymous));
            }
        }

        public void NotifyAuthChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private static IEnumerable<Claim>? ParseClaimsFromJwt(string jwt)
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
            await tokenStorage.SetTokens(accessToken, refreshToken);

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
            await tokenStorage.DeleteTokens();
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
        }
    }
}
