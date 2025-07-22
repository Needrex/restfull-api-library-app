using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Models
{
    public class Books
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        public int Price { get; set; }
        [Required]
        [MaxLength(10)]
        public int Stock { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Writer { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        public string Description { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }


        public int UserCreateId { get; set; }
        public Users UserCreate { get; set; }
        public int? UserUpdateId { get; set; }
        public Users UserUpdate { get; set; }
        public ICollection<ShoppingCart> ShoppingCart { get; set; }
    }
}