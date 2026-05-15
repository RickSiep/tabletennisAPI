namespace TableTennisShared.DTO.Token
{
    public class UserInfoWithTokens
    {
        public required int UserId { get; set; }
        public required string FirstName { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
