using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RestApiApp.Data;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Models;

namespace RestApiApp.Repositories
{
    public class UserOtpsRepository : GenericRepository<UserOtps, UserOtps>, IUserOtpsRepository
    {
        public UserOtpsRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async override Task<UserOtps> GetAsyc(Expression<Func<UserOtps, bool>> predicate)
        {
            var userOtps = await _context.UserOtps
            .AsNoTrackingWithIdentityResolution()
            .Include(o => o.Users)
            .FirstOrDefaultAsync(predicate);

            return userOtps;
        }
        
        public async Task<UserOtps> UpdateOTPStatusAsyc(int idOtp, UserOtps userOtps)
        {
            var findUserOtps = await _context.Set<UserOtps>().FindAsync(idOtp);
            if (findUserOtps == null) return findUserOtps;

            findUserOtps.IsUsed = userOtps.IsUsed;
            _context.Update(findUserOtps);
            await _context.SaveChangesAsync();

            return findUserOtps;
        }
    }
}