namespace TableTennisAPI.Models
{
    public class ExternalCredential
    {
        public int Id;
        public string Provider { get; set; } = string.Empty;
        public string ProviderUserId { get; set; } = string.Empty;
    }
}
