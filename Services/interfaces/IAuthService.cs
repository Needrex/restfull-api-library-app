using RestApiApp.Dtos;

namespace RestApiApp.InterfaceServices
{
    public interface IAuthService
    {
        Task<RegisterResponeDto> RegisterAsync(RegisterRequestDto user);
        Task<LoginResponeDto> LoginAsync(LoginRequestDto user);
        Task<LogoutResponeDto> LogoutAsync(int userId, LogoutRequestDto loginRequestDto);
        Task<GenerateOTPDto> GenerateOTPAsync(int userId);
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task<VerifyOtpResponeDto> VerifyOtpAsync(int userId, VerifyOtpRequestDto verifyOtpRequest);
        Task<ChangePasswordResponeDto> ChangePasswordAsync(int userId, ChangePasswordRequestDto ChangePasswordRequestDto);
    }
}