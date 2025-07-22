using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Dtos
{
    public class VerifyOtpRequestDto
    {
        [Required]
        public string Otp { get; set; }

    }

    public class VerifyOtpResponeDto
    {
        [Required]
        public AccessTokenResponseDto AccessToken { get; set; }
    }
}