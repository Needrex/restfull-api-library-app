using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TotalItems { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        public int BooksId { get; set; }
        public Books Books { get; set; }
        public int UserId { get; set; }
        public Users Users { get; set; }
    }
}