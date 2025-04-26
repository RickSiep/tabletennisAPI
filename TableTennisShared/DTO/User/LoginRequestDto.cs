using System.ComponentModel.DataAnnotations;

namespace TableTennisShared.DTO.User
{
    public class LoginRequestDto
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
