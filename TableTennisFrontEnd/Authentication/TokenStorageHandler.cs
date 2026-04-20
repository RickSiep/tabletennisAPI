using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace TableTennisFrontEnd.Authentication
{
    public class TokenStorageHandler(ProtectedLocalStorage storage)
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsInitialized { get; set; }

        public async Task InitializeAsync()
        {
            AccessToken = (await storage.GetAsync<string>("authToken")).Value;
            RefreshToken = (await storage.GetAsync<string>("refreshToken")).Value;

            IsInitialized = true;
        }

        public async Task SetTokens(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;

            await storage.SetAsync("authToken", accessToken);
            await storage.SetAsync("refreshToken", refreshToken);
        }

        public async Task DeleteTokens()
        {
            AccessToken = null;
            RefreshToken = null;

            await storage.DeleteAsync("authToken");
            await storage.DeleteAsync("refreshToken");
        }
    }
}
