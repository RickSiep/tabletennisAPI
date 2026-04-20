using System.Net;
using System.Net.Http.Headers;
using TableTennisShared.DTO.Token;

namespace TableTennisFrontEnd.Authentication
{
    public class TokenRefreshHandler(TokenStorageHandler tokenStorage, IHttpClientFactory factory) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    var refreshToken = tokenStorage.RefreshToken;
                    var authToken = tokenStorage.AccessToken;

                    if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(authToken))
                    {
                        return response;
                    }

                    var userId = ExtractUserIdFromToken(authToken);
                    if (userId < 0)
                    {
                        return response;
                    }

                    var newTokens = await RefreshAccessTokensAsync(userId, refreshToken, cancellationToken);

                    if (newTokens != null)
                    {
                        await tokenStorage.SetTokens(newTokens.AccessToken, newTokens.RefreshToken);

                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newTokens.AccessToken);
                        response = await base.SendAsync(request, cancellationToken);
                    }
                }
                catch (InvalidOperationException)
                {
                }
            }
            return response;
        }

        private int ExtractUserIdFromToken(string token)
        {
            try
            {
                var payload = token.Split('.')[1];
                var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(CustomAuthStateProvider.PadBase64(payload)));
                var keyValuePairs = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(json);

                if (keyValuePairs != null && keyValuePairs.TryGetValue("sub", out var userId))
                {
                    return int.Parse(userId.ToString());
                }
            }
            catch
            {
                // Token parsing failed
            }

            return -1;
        }

        private async Task<TokenResponseDto?> RefreshAccessTokensAsync(int userId, string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                var refreshTokenDto = new RefreshTokenRequestDto()
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                };

                var apiClient = new ApiClient(factory.CreateClient());
                var response = await apiClient.PostJsonAsync("/auth/refresh-token", refreshTokenDto);
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                return await response.Content.ReadFromJsonAsync<TokenResponseDto>(cancellationToken: cancellationToken);
            }
            catch
            {

            }

            return null;
        }
    }
}
