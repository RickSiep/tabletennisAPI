namespace TableTennisAPI.Models
{
    public class ExternalLogin
    {
        Guid UserId;
        string Provider { get; set; } = string.Empty;
        string ProviderUserId { get; set; } = string.Empty;
    }
}
