namespace TableTennisFrontEnd.Authentication
{
    public class TokenStorageHandler
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public void SetTokens(string access, string refresh)
        {
            AccessToken = access;
            RefreshToken = refresh;
        }

        public void Clear()
        {
            AccessToken = null;
            RefreshToken = null;
        }
    }
}
