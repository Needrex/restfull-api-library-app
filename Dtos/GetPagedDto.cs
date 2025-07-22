using System.ComponentModel.DataAnnotations;

namespace RestApiApp.Dtos
{
    public class GetPagedResponeDto<TDto> where TDto : class
    {
        [Required]
        public List<TDto> Items { get; set; }
        [Required]
        public int TotalItems { get; set; }
        [Required]
        public int Page { get; set; }
        [Required]
        public int PageSize { get; set; }
    }

    public class GetPagedRequestDto 
    {
        [Required]
        public int Page { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}