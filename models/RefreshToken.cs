using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }


        public int UserId { get; set; }        
        public Users User { get; set; }
    }
}