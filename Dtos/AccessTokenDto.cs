using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Dtos
{

    public class AccessTokenResponseDto
    {
        [Required]
        public string AccessToken { get; set; }
    }

    public class AccessTokenRequestDto 
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}