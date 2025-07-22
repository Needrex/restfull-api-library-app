using System.Security.Claims;
using RestApiApp.InterfaceServices;
using Microsoft.IdentityModel.Tokens;
using RestApiApp.Dtos;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using RestApiApp.Utils;
using RestApiApp.Repositories;

namespace RestApiApp.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public TokenService(IConfiguration configuration,
                            IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public AccessTokenResponseDto GenerateAccessTokenAsync(AccessTokenRequestDto accessTokenRequestDto, bool isVerified = false)
        {
            const string KEYUSERID = "UserId";
            const string KEYVERIFIED = "IsVerified";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, accessTokenRequestDto.Username),
                new Claim(ClaimTypes.Email, accessTokenRequestDto.Email),
                new Claim(KEYUSERID, accessTokenRequestDto.UserId.ToString()),
                new Claim(KEYVERIFIED, isVerified.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            var accessToken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var accessTokenResponseDto = new AccessTokenResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken)
            };
            return accessTokenResponseDto;
        }


        public string GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        

        public async Task<RegenerateAccessTokenResponeDto> RegenerateAccessTokenAsync(RegenerateAccessTokenRequestDto RegenerateAccessTokenRequestDto)
        {
            var checkRefresh = await _refreshTokenRepository.GetAsyc(r => r.Token == RegenerateAccessTokenRequestDto.refreshToken && r.ExpiresAt != DateTime.Now)
                                    ?? throw new UnauthorizedAccessException("You are not authenticated");

            if (!PasswordHasher.VerifyPassword(RegenerateAccessTokenRequestDto.refreshToken, checkRefresh.Token)
                && checkRefresh.IsRevoked == true)
            {
                throw new UnauthorizedAccessException("You are not authenticated");
            }


            var newAccessToken = GenerateAccessTokenAsync(new AccessTokenRequestDto
            {
                Username = checkRefresh.User.Username,
                Email = checkRefresh.User.Email,
                UserId = checkRefresh.UserId
            });

            var dataToken = new RegenerateAccessTokenResponeDto
            {
                AccessToken = newAccessToken,
                RefreshToken = RegenerateAccessTokenRequestDto.refreshToken
            };
            return dataToken;
        }   
    }
}
    
