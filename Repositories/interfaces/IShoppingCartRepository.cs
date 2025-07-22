using System.Linq.Expressions;
using RestApiApp.Models;

namespace RestApiApp.InterfaceRepositories
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart, ShoppingCartResponeDto>
    {
        Task<List<ShoppingCart>> GetAsycFilterData(Expression<Func<ShoppingCart, bool>> predicate);
        Task<ShoppingCart> DeleteAsyc(int shoppingCartId);
        Task<ShoppingCart> GetAsyc(Expression<Func<ShoppingCart, bool>> predicate);
        Task<ICollection<ShoppingCart>> GetAsycAllAsync(int userId);
    }
}