using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
namespace TableTennisFrontEnd
{
    public class ApiClient(HttpClient client, AuthenticationStateProvider authProvider)
    {
        public async Task<IAsyncEnumerable<T>> GetAllFromJsonAsync<T>(string path)
        {
            return client.GetFromJsonAsAsyncEnumerable<T>(path);
        }

        public async Task<T> GetFromJsonAsync<T>(string path) => await client.GetFromJsonAsync<T>(path);

        public async Task<T?> GetFromJsonAsyncAuthorized<T>(string path)
        {
            var user = (await authProvider.GetAuthenticationStateAsync()).User;
            var accessToken = user.FindFirst("access_token")?.Value;
            
            if (accessToken == null)
            {
                return default;
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return await client.GetFromJsonAsync<T>(path);
        }

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string path, T value)
        {
            return await client.PostAsJsonAsync(path, value);
        }

        public async Task<TOut> PostJsonAsyncWithResponseModel<TIn, TOut>(string path, TIn postModel)
        {
            var response = await client.PostAsJsonAsync(path, postModel);
            if (response == null || !response.IsSuccessStatusCode)
            {
                return default;
            }

            return await response.Content.ReadFromJsonAsync<TOut>();
        }

        public async Task<HttpResponseMessage> DeleteRouteAuthorizedAsync(string path, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync(path);
            return response;
        }
    }
}
