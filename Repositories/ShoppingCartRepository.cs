using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RestApiApp.Data;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Models;
using RestApiApp.Utils;

namespace RestApiApp.Repositories
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart, ShoppingCartResponeDto>, IShoppingCartRepository
    {
        public ShoppingCartRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<List<ShoppingCart>> GetAsycFilterData(Expression<Func<ShoppingCart, bool>> predicate)
        {
            var shoppingCart = await _context.ShoppingCart
            .Where(predicate)
            .AsNoTrackingWithIdentityResolution()
            .Include(s => s.Books)
            .ToListAsync();

            return shoppingCart;
        }


        public override async Task<ShoppingCart> GetAsyc(Expression<Func<ShoppingCart, bool>> predicate)
        {
            var result = await _context.ShoppingCart
            .AsNoTrackingWithIdentityResolution()
            .Include(s => s.Books)
            .FirstOrDefaultAsync(predicate);
            
            return result;
        }


        public async Task<ICollection<ShoppingCart>> GetAsycAllAsync(int userId)
        {
            var result = await _context.ShoppingCart
            .Where(s => s.UserId == userId)
            .AsNoTrackingWithIdentityResolution()
            .Include(s => s.Books)
            .ToListAsync();
            
            return result;
        }


        public override async Task<ShoppingCart> DeleteAsyc(int shoppingCartId)
        {
            var shoppingCart = await _context.ShoppingCart
                                    .Include(s => s.Books)
                                    .FirstOrDefaultAsync(s => s.Id == shoppingCartId);
            if (shoppingCart == null) return shoppingCart;

            _context.Remove(shoppingCart);
            await _context.SaveChangesAsync();

            return shoppingCart;
        }


        public override async Task<ShoppingCart> UpdateAsync(int shoppingCartId, ShoppingCart shoppingCart)
        {
            var result = await _context.ShoppingCart
                                    .Include(s => s.Books)
                                    .FirstOrDefaultAsync(s => s.Id == shoppingCartId);
            if (result == null) return result;

            ObjectUpdateAsyncr.UpdateAsyncNonNullProperties(shoppingCart, result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}