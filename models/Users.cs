using System.ComponentModel.DataAnnotations;
namespace RestApiApp.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        [MinLength(3)]
        [MaxLength(50)]
        public string Email { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? LastLoginAt { get; set; }


        public RefreshToken RefreshToken { get; set; }
        public ICollection<Books> CreatedBooks { get; set; }
        public ICollection<Books> UpdatedBooks { get; set; }
        public ICollection<UserOtps> UserOtps { get; set; }
        public ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}