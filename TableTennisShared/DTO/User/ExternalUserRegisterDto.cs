using System.ComponentModel.DataAnnotations;

namespace TableTennisShared.DTO.User
{
    public class ExternalUserRegisterDto
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Provider { get; set; } = string.Empty;
        [Required]
        public string ProviderUserId { get; set; } = string.Empty;
    }
}
