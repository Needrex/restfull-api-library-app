using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiApp.Dtos;
using RestApiApp.InterfaceServices;
using RestApiApp.Utils;

namespace RestApiApp.Models
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksServices _bookService;
        public BooksController(IBooksServices bookService)
        {
            _bookService = bookService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(new ApiRespone<List<BooksResponeDto>>(true, "Success Get all book", books));
        }

        [Authorize]
        [HttpGet("Get-paged")]
        public async Task<IActionResult> GetBooksPaged([FromQuery] GetPagedRequestDto GetPagedRequestDto)
        {
            var books = await _bookService.GetPagedAsyc(GetPagedRequestDto);
            return Ok(new ApiRespone<GetPagedResponeDto<BooksResponeDto>>(true, "Success Get all book", books));
        }

        [Authorize]
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBook([FromRoute] int bookId)
        {
            var book = await _bookService.GetAsyc(bookId);
            return Ok(new ApiRespone<BooksResponeDto>(true, "Success Get user", book));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BooksRequestDto booksDto)
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);

            var book = await _bookService.AddAsyc(booksDto, userIdInt);
            return CreatedAtAction(nameof(GetBooks), new ApiRespone<BooksResponeDto>(true, "Success Get all book", book));
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> DeleteBook([FromBody] int booksByIdDto)
        {
            var book = await _bookService.DeleteAsyc(booksByIdDto);
            return Ok(new ApiRespone<BooksResponeDto>(true, "Success Delete user", book));
        }

        [Authorize]
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int bookId, [FromBody] BooksRequestDto booksDto)
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);

            var book = await _bookService.UpdateAsync(userIdInt, bookId, booksDto);
            return CreatedAtAction(nameof(GetBooks), new ApiRespone<BooksResponeDto>(true, "Success update book", book));
        }
        
        [Authorize]
        [HttpPatch("adjust-stock/{bookId}")]
        public async Task<IActionResult> AdjustStockAsync([FromRoute] int bookId ,[FromBody] AdjustStockRequestDto adjustStockRequestDto)
        {   
            var book = await _bookService.AdjustStockAsync(bookId, adjustStockRequestDto);
            return CreatedAtAction(nameof(GetBooks),new ApiRespone<AdjustStockResponeDto>(true, "Success update stock book", book));
        }
    }
}