using RestApiApp.Exceptions;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Models;
using RestApiApp.Repositories;

namespace RestApiApp.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IBooksRepository _booksRepository;
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository,
                                    IBooksRepository booksRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _booksRepository = booksRepository;
        }

        public async Task<ShoppingCartResponeDto> AddAsyc(int userId, ShoppingCartRequestDto shoppingCartRequestDto)
        {
            var GetAsycBook = await _booksRepository.GetAsyc(b => b.Id == shoppingCartRequestDto.BookId)
                            ?? throw new NotFoundException("Book not found!");

            if (shoppingCartRequestDto.TotalItems > GetAsycBook.Stock)
            {
                throw new ConflictException("Insufficient stock!");
            }

            int totalStock = GetAsycBook.Stock - shoppingCartRequestDto.TotalItems;
            var bookEntity = new Books
            {
                Stock = totalStock
            };
            await _booksRepository.UpdateAsync(GetAsycBook.Id, bookEntity);


            var shoppingCartEntity = new ShoppingCart
            {
                TotalItems = shoppingCartRequestDto.TotalItems,
                BooksId = GetAsycBook.Id,
                UserId = userId,
            };
            var AddAsycToCart = await _shoppingCartRepository.AddAsyc(shoppingCartEntity);


            var totalPrice = GetAsycBook.Price * AddAsycToCart.TotalItems;
            var shoppingCartResponeDto = new ShoppingCartResponeDto
            {
                Id = AddAsycToCart.Id,
                Name = GetAsycBook.Name,
                Price = GetAsycBook.Price,
                TotalItems = AddAsycToCart.TotalItems,
                TotalPrice = totalPrice
            };
            return shoppingCartResponeDto;
        }


        public async Task<ShoppingCartResponeDto> GetAsyc(int userId, int shoppingCartId)
        {
            var GetAsycShoppingCart = await _shoppingCartRepository.GetAsyc(s => s.Id == shoppingCartId && s.Users.Id == userId)
                            ?? throw new NotFoundException("Shopping cart not found!");

            var totalPrice = GetAsycShoppingCart.Books.Price * GetAsycShoppingCart.TotalItems;
            var shoppingCartResponeDto = new ShoppingCartResponeDto
            {
                Id = GetAsycShoppingCart.Id,
                Name = GetAsycShoppingCart.Books.Name,
                Price = GetAsycShoppingCart.Books.Price,
                TotalItems = GetAsycShoppingCart.TotalItems,
                TotalPrice = totalPrice
            };
            return shoppingCartResponeDto;
        }


        public async Task<List<ShoppingCartResponeDto>> GetAsycAllAsync(int userId)
        {
            var GetAsycShoppingCart = await _shoppingCartRepository.GetAsycAllAsync(userId)
                            ?? throw new NotFoundException("Shopping cart not found!");

            var shoppingCartResponeDto = new List<ShoppingCartResponeDto>();
            foreach (var cart in GetAsycShoppingCart)
            {
                var totalPrice = cart.Books.Price * cart.TotalItems;
                shoppingCartResponeDto.Add(new ShoppingCartResponeDto
                {
                    Id = cart.Id,
                    Name = cart.Books.Name,
                    Price = cart.Books.Price,
                    TotalItems = cart.TotalItems,
                    TotalPrice = totalPrice
                });
            }
            return shoppingCartResponeDto;
        }


        public async Task<ShoppingCartResponeDto> DeleteAsyc(int shoppingCartId)
        {
            var GetAsycShoppingCart = await _shoppingCartRepository.DeleteAsyc(shoppingCartId)
                            ?? throw new NotFoundException("Shopping cart not found!");

            var totalStock = GetAsycShoppingCart.TotalItems + GetAsycShoppingCart.Books.Stock;
            var bookEntity = new Books
            {
                Stock = totalStock
            };
            await _booksRepository.UpdateAsync(GetAsycShoppingCart.Books.Id, bookEntity);


            var totalPrice = GetAsycShoppingCart.Books.Price * GetAsycShoppingCart.TotalItems;
            var shoppingCartResponeDto = new ShoppingCartResponeDto
            {
                Id = GetAsycShoppingCart.Id,
                Name = GetAsycShoppingCart.Books.Name,
                Price = GetAsycShoppingCart.Books.Price,
                TotalItems = GetAsycShoppingCart.TotalItems,
                TotalPrice = totalPrice
            };
            return shoppingCartResponeDto;
        }
        

        public async Task<ShoppingCartResponeDto> UpdateAsync(ShoppingCartRequestUpdateDto shoppingCartRequestUpdateAsyncDto)
        {
            var shoppingCartEntity = new ShoppingCart
            {
                TotalItems = shoppingCartRequestUpdateAsyncDto.TotalItems
            };
            var GetAsycShoppingCart = await _shoppingCartRepository.UpdateAsync(shoppingCartRequestUpdateAsyncDto.Id, shoppingCartEntity)
                            ?? throw new NotFoundException("Shopping cart not found!");

            var totalStock = GetAsycShoppingCart.TotalItems + GetAsycShoppingCart.Books.Stock;
            var bookEntity = new Books
            {
                Stock = totalStock
            };
            await _booksRepository.UpdateAsync(GetAsycShoppingCart.Books.Id, bookEntity);


            var totalPrice = GetAsycShoppingCart.Books.Price * GetAsycShoppingCart.TotalItems;
            var shoppingCartResponeDto = new ShoppingCartResponeDto
            {
                Id = GetAsycShoppingCart.Id,
                Name = GetAsycShoppingCart.Books.Name,
                Price = GetAsycShoppingCart.Books.Price,
                TotalItems = GetAsycShoppingCart.TotalItems,
                TotalPrice = totalPrice
            };
            return shoppingCartResponeDto;
        }
    }
}