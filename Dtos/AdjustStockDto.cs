namespace RestApiApp.Dtos
{
    public class AdjustStockRequestDto
    {
        public int AdjustBy { get; set; }
    }

    public class AdjustStockResponeDto
    {
        public int AdjustBy { get; set; }
        public BooksResponeDto Book { get; set; }
    }
}