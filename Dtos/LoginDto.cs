using System.ComponentModel.DataAnnotations;


namespace RestApiApp.Dtos
{
    public class LoginRequestDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }
    }


    public class LoginResponeDto
    {
        [Required]
        public AccessTokenResponseDto AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public bool StatusAuthentication { get; set; }
    }
}