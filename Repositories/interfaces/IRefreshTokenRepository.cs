using RestApiApp.Dtos;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Models;

namespace RestApiApp.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, RegenerateAccessTokenResponeDto>
    {
        public Task<RefreshToken> UpdateRevokedStatusAsync(int idRefreshToken, RefreshToken refreshToken);
    }
}