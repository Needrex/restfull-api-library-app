using RestApiApp.Dtos;

namespace RestApiApp.InterfaceServices
{
    public interface ITokenService
    {
        public AccessTokenResponseDto GenerateAccessTokenAsync(AccessTokenRequestDto user, bool isVerified = false);
        public string GenerateRefreshTokenAsync();
        public Task<RegenerateAccessTokenResponeDto> RegenerateAccessTokenAsync(RegenerateAccessTokenRequestDto refreshToken);
    }
}