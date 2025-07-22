using System.ComponentModel.DataAnnotations;
namespace RestApiApp.Dtos
{
    public class RegenerateAccessTokenRequestDto
    {
        [Required]
        public string refreshToken { get; set; }
    }

    public class RegenerateAccessTokenResponeDto
    {
        [Required]
        public AccessTokenResponseDto AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        [Required]
        public bool StatusAuthentication { get; set; }
    }
}