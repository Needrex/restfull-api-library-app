namespace RestApiApp.InterfaceRepositories
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartResponeDto> AddAsyc(int userId, ShoppingCartRequestDto shoppingCartRequestDto);
        Task<ShoppingCartResponeDto> GetAsyc(int userId, int shoppingCartId);
        Task<List<ShoppingCartResponeDto>> GetAsycAllAsync(int userId);

        Task<ShoppingCartResponeDto> DeleteAsyc(int shoppingCartId);
        Task<ShoppingCartResponeDto> UpdateAsync(ShoppingCartRequestUpdateDto shoppingCartRequestUpdateDto);

    }
}