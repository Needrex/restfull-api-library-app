using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Dtos
{
    public class LogoutRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }

    public class LogoutResponeDto
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string Username { get; set; } 
        
        [Required]
        public string Email {get; set;} 
    }
}