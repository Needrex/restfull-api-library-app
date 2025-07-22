using RestApiApp.Dtos;
using RestApiApp.Exceptions;
using RestApiApp.InterfaceServices;
using RestApiApp.Models;
using RestApiApp.Repositories;

namespace RestApiApp.Services
{
    public class BooksService : IBooksServices
    {
        private readonly IBooksRepository _bookRepository;
        private readonly ILogger<BooksService> _logger;
        public BooksService(IBooksRepository bookRepository, ILogger<BooksService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<List<BooksResponeDto>> GetAllAsync()
        {
            var booksEntity = await _bookRepository.GetAsycAllAsync();
            var booksDto = new List<BooksResponeDto>();
            foreach (var book in booksEntity)
            {
                booksDto.Add(
                    new BooksResponeDto
                    {
                        Id = book.Id,
                        Name = book.Name,
                        Description = book.Description,
                        Writer = book.Writer,
                        Price = book.Price,
                        Stock = book.Stock
                    }
                );
            }

            return booksDto;
        }

        public async Task<BooksResponeDto> GetAsyc(int bookId)
        {
            var GetAsycBook = await _bookRepository.GetAsyc(b => b.Id == bookId)
                        ?? throw new NotFoundException("Book not found!");
            var book = new BooksResponeDto
            {
                Id = GetAsycBook.Id,
                Name = GetAsycBook.Name,
                Description = GetAsycBook.Description,
                Writer = GetAsycBook.Writer,
                Price = GetAsycBook.Price,
                Stock = GetAsycBook.Stock
            };
            return book;
        }

        public async Task<GetPagedResponeDto<BooksResponeDto>> GetPagedAsyc(GetPagedRequestDto GetAsycPagedRequestDto)
        {
            int paged = GetAsycPagedRequestDto.Page < 1 ? 1 : GetAsycPagedRequestDto.Page;
            int pageSize = GetAsycPagedRequestDto.PageSize > 20 ? 20 : GetAsycPagedRequestDto.PageSize;

            var GetAsycBook = await _bookRepository.GetAsycPaged(b => new BooksResponeDto
            {
                Id = b.Id,
                Description = b.Description,
                Name = b.Name,
                Price = b.Price,
                Stock = b.Stock,
                Writer = b.Writer
            }, paged, pageSize)
            ?? throw new NotFoundException("Book not found!");

            return GetAsycBook;
        }

        public async Task<BooksResponeDto> AddAsyc(BooksRequestDto book, int userCreateId)
        {
            var bookEntity = new Books
            {
                Name = book.Name,
                Description = book.Description,
                Writer = book.Writer,
                Price = book.Price,
                UserCreateId = userCreateId
            };

            var bookAddAsyc = await _bookRepository.AddAsyc(bookEntity);
            var booksDto = new BooksResponeDto
            {
                Id = bookAddAsyc.Id,
                Name = bookAddAsyc.Name,
                Description = bookAddAsyc.Description,
                Writer = bookAddAsyc.Writer,
                Price = bookAddAsyc.Price,
                Stock = bookAddAsyc.Stock
            };
            return booksDto;
        }

        public async Task<BooksResponeDto> DeleteAsyc(int bookId)
        {
            var bookDelete = await _bookRepository.DeleteAsyc(bookId) ?? throw new NotFoundException("Book not found!");
            var booksDto = new BooksResponeDto
            {
                Id = bookDelete.Id,
                Name = bookDelete.Name,
                Description = bookDelete.Description,
                Writer = bookDelete.Writer,
                Price = bookDelete.Price,
                Stock = bookDelete.Stock
            };
            return booksDto;
        }

        public async Task<BooksResponeDto> UpdateAsync(int userUpdateId, int bookId, BooksRequestDto book)
        {
            var bookEntity = new Books
            {
                Name = book.Name,
                Description = book.Description,
                Writer = book.Writer,
                Price = book.Price,
                UserUpdateId = userUpdateId,
                UpdateAt = DateTime.Now
            };

            var bookUpdateAsync = await _bookRepository.UpdateAsync(bookId, bookEntity) ?? throw new NotFoundException("Book not found!");
            var booksDto = new BooksResponeDto
            {
                Id = bookUpdateAsync.Id,
                Name = bookUpdateAsync.Name,
                Description = bookUpdateAsync.Description,
                Writer = bookUpdateAsync.Writer,
                Price = bookUpdateAsync.Price,
                Stock = bookUpdateAsync.Stock
            };
            return booksDto;
        }

        public async Task<AdjustStockResponeDto> AdjustStockAsync(int bookId, AdjustStockRequestDto adjustStockRequestDto)
        {
            var bookEntity = new Books
            {
                Stock = adjustStockRequestDto.AdjustBy
            };
            var adjustStockBook = await _bookRepository.AdjustStockAsync(bookId, bookEntity)
                                        ?? throw new NotFoundException("Book not found!");


            var adjustStockResponeDto = new AdjustStockResponeDto
            {
                AdjustBy = adjustStockRequestDto.AdjustBy,
                Book = new BooksResponeDto
                {
                    Id = adjustStockBook.Id,
                    Name = adjustStockBook.Name,
                    Price = adjustStockBook.Price,
                    Stock = adjustStockBook.Stock,
                    Writer = adjustStockBook.Writer,
                    Description = adjustStockBook.Description,
                }
            };
            return adjustStockResponeDto;
        }
    }
}