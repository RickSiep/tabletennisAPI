using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using TableTennisFrontEnd.Authentication;
using TableTennisShared.DTO.Token;
namespace TableTennisFrontEnd
{
    public class ApiClient(IHttpClientFactory factory, AuthState authState, NavigationManager navigationManager)
    {
        private readonly HttpClient _api = factory.CreateClient("api");
        private readonly HttpClient _localClient = factory.CreateClient("local");
        public async Task<IAsyncEnumerable<T>> GetAllFromJsonAsync<T>(string path)
        {
            return _api.GetFromJsonAsAsyncEnumerable<T>(path);
        }

        public async Task<T> GetFromJsonAsync<T>(string path) => await _api.GetFromJsonAsync<T>(path);

        public async Task<T?> GetFromJsonAsyncAuthorized<T>(string path)
        {
            if (authState.AccessToken == null && authState.RefreshToken != null)
            {
                await TryRefreshAsync();
            }

            try
            {
                _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authState.AccessToken);
                return await _api.GetFromJsonAsync<T>(path);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshed = await TryRefreshAsync();

                if (!refreshed)
                {
                    navigationManager.NavigateTo("/account/logout", true);
                    return default;
                }

                _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authState.AccessToken);
                return await _api.GetFromJsonAsync<T>(path);
            }
        }

        private async Task<bool> TryRefreshAsync()
        {
            try
            {
                var response = await PostJsonAsyncWithResponseModel<string, TokenResponseDto>("/auth/refresh-token-from-cookie", authState.RefreshToken);
                if (response == null)
                {
                    navigationManager.NavigateTo("/account/logout", true);
                    return false;
                }

                authState.SetTokens(response.AccessToken, response.RefreshToken);
                await _localClient.PostAsJsonAsync("/account/RefreshToken", response);
            }
            catch (Exception e)
            {
                string yee = e.InnerException.Message;
            }


            return true;
        }

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string path, T value)
        {
            return await _api.PostAsJsonAsync(path, value);
        }

        public async Task<TOut> PostJsonAsyncWithResponseModel<TIn, TOut>(string path, TIn postModel)
        {
            var response = await _api.PostAsJsonAsync(path, postModel);
            if (response == null || !response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<TOut>();
        }

        public async Task<HttpResponseMessage> DeleteRouteAuthorizedAsync(string path, string token)
        {
            _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _api.DeleteAsync(path);
            return response;
        }
    }
}
