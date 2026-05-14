namespace TableTennisAPI.Models
{
    public class LocalCredential
    {
        public Guid UserId { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
