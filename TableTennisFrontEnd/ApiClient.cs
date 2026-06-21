using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using TableTennisFrontEnd.Authentication;
using TableTennisShared.DTO.Token;
namespace TableTennisFrontEnd
{
    public class ApiClient(IHttpClientFactory factory, AuthState authState, NavigationManager navigationManager)
    {
        private readonly HttpClient _api = factory.CreateClient("api");
        public IAsyncEnumerable<T?> GetAllFromJsonAsync<T>(string path) => _api.GetFromJsonAsAsyncEnumerable<T>(path);
        
        public async Task<T> GetFromJsonAsync<T>(string path) => await _api.GetFromJsonAsync<T>(path);

        public async Task<T?> GetFromJsonAsyncAuthorized<T>(string path, CancellationTokenSource cts = null)
        {
            if (authState.AccessToken == null && authState.RefreshToken != null)
            {
                await TryRefreshAsync();
            }

            cts ??= new CancellationTokenSource();

            try
            {
                _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authState.AccessToken);
                var result = await _api.GetFromJsonAsync<T>(path, cts.Token);
                return result ?? default;
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
            catch (JsonException exception)
            {
                Console.WriteLine(exception.InnerException);
                return default;
            }
        }

        private async Task<bool> TryRefreshAsync()
        {
            try
            {
                var response = await PostJsonAsyncWithContentReturn<string>("/auth/get-access-token-from-refresh", authState.RefreshToken);
                if (string.IsNullOrEmpty(response))
                {
                    navigationManager.NavigateTo("/account/logout", true);
                    return false;
                }

                authState.AccessToken = response;  
                //await _localClient.PostAsJsonAsync("/account/RefreshToken", response);
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

        public async Task<string> PostJsonAsyncWithContentReturn<TIn>(string path, TIn postModel)
        {
            var response = await _api.PostAsJsonAsync(path, postModel);
            if (response == null || !response.IsSuccessStatusCode)
            {
                return string.Empty;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> DeleteRouteAuthorizedAsync(string path, string token)
        {
            _api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _api.DeleteAsync(path);
            return response;
        }
    }
}
