using RestApiApp.InterfaceRepositories;
using RestApiApp.Data;
using Microsoft.EntityFrameworkCore;
using RestApiApp.Models;
using RestApiApp.Utils;
using System.Linq.Expressions;

namespace RestApiApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        async public Task<Users> CreateAsyc(Users user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        async public Task<Users> GetAsyc(Expression<Func<Users, bool>> predicate)
        {
            var user = await _context.Users
                .Include(u => u.RefreshToken)
                .FirstOrDefaultAsync(predicate);

            return user;
        }

        public async Task<Users> UpdateAsync(int userId, Users user)
        {
            var result = await _context.Set<Users>().FindAsync(userId);
            if (result == null) return result;

            ObjectUpdateAsyncr.UpdateAsyncNonNullProperties(user, result);
            await _context.SaveChangesAsync();

            return result;
        }
        

        public async Task<Users> UpdateLastLoginAsync(Users user)
        {
            user.LastLoginAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return user;
        }
    }
}
