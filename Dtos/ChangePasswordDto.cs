using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Dtos
{
    public class ChangePasswordRequestDto
    {
        [Required]
        public string NewPassword { get; set; }
    }

    public class ChangePasswordResponeDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
    }
}