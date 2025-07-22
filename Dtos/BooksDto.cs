using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Dtos
{
    public class BooksRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Writer { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        public string Description { get; set; }
    }

    public class BooksResponeDto
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Writer { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(200)]
        public string Description { get; set; }
    }
    
}