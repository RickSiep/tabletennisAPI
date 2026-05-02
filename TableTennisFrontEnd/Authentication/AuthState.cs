namespace TableTennisFrontEnd.Authentication
{
    public class AuthState
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
