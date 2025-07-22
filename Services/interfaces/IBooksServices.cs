using RestApiApp.Dtos;

namespace RestApiApp.InterfaceServices
{
    public interface IBooksServices
    {
        Task<BooksResponeDto> AddAsyc(BooksRequestDto book, int userCreateId);
        Task<List<BooksResponeDto>> GetAllAsync();
        Task<BooksResponeDto> GetAsyc(int bookId);
        Task<BooksResponeDto> DeleteAsyc(int bookId);
        Task<BooksResponeDto> UpdateAsync(int userUpdateAsyncId, int bookId, BooksRequestDto book);
        Task<GetPagedResponeDto<BooksResponeDto>> GetPagedAsyc(GetPagedRequestDto GetAsycPagedRequestDto);
        Task<AdjustStockResponeDto> AdjustStockAsync(int bookId, AdjustStockRequestDto adjustStockRequestDto);
    }
}