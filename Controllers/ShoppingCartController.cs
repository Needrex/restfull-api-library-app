using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Utils;

namespace RestApiApp.Models
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAsyc([FromBody] ShoppingCartRequestDto shoppingCartRequestDto)
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);

            var result = await _shoppingCartService.AddAsyc(userIdInt, shoppingCartRequestDto);
            return CreatedAtAction(
            nameof(GetAsyc),
            new { shoppingCartId = result.Id }
            , new ApiRespone<ShoppingCartResponeDto>(
                true,
                "Cart success created!",
                result
            ));
        }


        [Authorize]
        [HttpGet("{shoppingCartId}")]
        public async Task<IActionResult> GetAsyc(int shoppingCartId)
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);
            
            var result = await _shoppingCartService.GetAsyc(userIdInt, shoppingCartId);
            return Ok(new ApiRespone<ShoppingCartResponeDto>(
                true,
                "Cart success displayed!",
                result
            ));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAsycAllAsync()
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);
            
            var result = await _shoppingCartService.GetAsycAllAsync(userIdInt);
            return Ok(new ApiRespone<List<ShoppingCartResponeDto>>(
                true,
                "Cart success displayed!",
                result
            ));
        }


        [Authorize]
        [HttpDelete("{shoppingCartId}")]
        public async Task<IActionResult> DeleteAsyc(int shoppingCartId)
        {
            var result = await _shoppingCartService.DeleteAsyc(shoppingCartId);
            return Ok(new ApiRespone<ShoppingCartResponeDto>(
                true,
                "Cart success DeleteAsycd!",
                result
            ));
        }
        

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateAsync(ShoppingCartRequestUpdateDto shoppingCartRequestUpdateDto)
        {
            var result = await _shoppingCartService.UpdateAsync(shoppingCartRequestUpdateDto);
            return Ok(new ApiRespone<ShoppingCartResponeDto>(
                true,
                "Cart success UpdateAsyncd!",
                result
            ));
        }
    }
}