using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Dtos
{
    public class RegisterRequestDto
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

        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string Address { get; set; }
    }

    public class RegisterResponeDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(50)]
        public string Address { get; set; }
    }
}