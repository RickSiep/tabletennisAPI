namespace TableTennisFrontEnd
{
    public class ApiClient(HttpClient client)
    {
        public Task<T> GetFromJsonAsync<T>(string path)
        {
            return client.GetFromJsonAsync<T>(path);
        }

        public Task<HttpResponseMessage> PostJsonAsync<T>(string path, T value)
        {
            return client.PostAsJsonAsync(path, value);
        }
    }
}
