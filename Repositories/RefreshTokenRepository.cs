using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RestApiApp.Data;
using RestApiApp.Dtos;
using RestApiApp.Models;

namespace RestApiApp.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken, RegenerateAccessTokenResponeDto>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(AppDbContext appDbContext) : base(appDbContext) { }

        public async Task<RefreshToken> UpdateRevokedStatusAsync(int idRefreshToken, RefreshToken refreshToken)
        {
            var result = await _context.Set<RefreshToken>().FindAsync(idRefreshToken);
            if (result == null) return result;

            result.Token = refreshToken.Token;
            result.IsRevoked = refreshToken.IsRevoked;
            result.RevokedAt = refreshToken.RevokedAt;
            _context.Update(result);
            await _context.SaveChangesAsync();

            return result;
        }
        
        async public override Task<RefreshToken> GetAsyc(Expression<Func<RefreshToken, bool>> predicate)
        {
            var refreshToken = await _context.RefreshToken
            .AsNoTrackingWithIdentityResolution()
            .Include(r => r.User)
            .FirstOrDefaultAsync(predicate);
            
            return refreshToken;
        }
    }
}