
using RestApiApp.Data;
using RestApiApp.Dtos;
using RestApiApp.Models;

namespace RestApiApp.Repositories
{
    public class BooksRepository : GenericRepository<Books, BooksResponeDto>, IBooksRepository
    {
        public BooksRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<Books> AdjustStockAsync(int idBook, Books book)
        {
            var result = await _context.Books.FindAsync(idBook);
            if (result == null) return result;

            result.Stock += book.Stock;
            _context.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }
        
    }
}