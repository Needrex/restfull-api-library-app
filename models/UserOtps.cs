using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Models
{
    public enum OtpType
    {
        EmailVerification,
        PasswordReset,
        TwoFactor
    }

    public class UserOtps
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string OtpCode { get; set; }
        [Required]
        public OtpType OtpType { get; set; }
        public bool IsUsed { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiredAt { get; set; } = DateTime.Now.AddMinutes(8);

        
        public int UserId { get; set; }
        public Users Users { get; set; }
    }
}