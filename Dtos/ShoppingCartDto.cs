using System.ComponentModel.DataAnnotations;

namespace RestApiApp
{
    public class ShoppingCartRequestDto
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int TotalItems { get; set; }
    }

    public class ShoppingCartResponeDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int TotalItems { get; set; }
        [Required]
        public int TotalPrice { get; set; }
    }

        public class ShoppingCartRequestUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int TotalItems { get; set; }
    }
}