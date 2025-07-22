using RestApiApp.Dtos;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Models;

namespace RestApiApp.Repositories
{
    public interface IBooksRepository : IGenericRepository<Books, BooksResponeDto>
    {
        Task<Books> AdjustStockAsync(int idBook, Books book);
    } 
        
}