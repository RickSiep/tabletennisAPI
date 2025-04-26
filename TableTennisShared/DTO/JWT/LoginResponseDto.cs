using System.Text.Json.Serialization;

namespace TableTennisShared.DTO.JWT
{
    public class LoginResponseDto
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
