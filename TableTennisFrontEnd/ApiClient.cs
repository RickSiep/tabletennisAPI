namespace TableTennisFrontEnd
{
    public class ApiClient(HttpClient client)
    {
        public async Task<IAsyncEnumerable<T>> GetAllFromJsonAsync<T>(string path)
        {
            return client.GetFromJsonAsAsyncEnumerable<T>(path);
        }

        public async Task<T> GetFromJsonAsync<T>(string path)
        {
            return await client.GetFromJsonAsync<T>(path);
        }

        public async Task<T> GetFromJsonAsyncAuthorized<T>(string path, string token)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
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
    }
}
