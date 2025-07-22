using Microsoft.AspNetCore.Mvc;
using RestApiApp.InterfaceServices;
using RestApiApp.Utils;
using RestApiApp.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace RestApiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequestDto RegisterRequestDto)
        {
            var user = await _authService.RegisterAsync(RegisterRequestDto);
            return Created(string.Empty, new ApiRespone<RegisterResponeDto>(
                true,
                "Register success",
                user
            ));
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto user)
        {
            var token = await _authService.LoginAsync(user);
            string loginMessage = (token.StatusAuthentication == true) ? "You are authenticated!" : "Login success!";
            return Ok(new ApiRespone<LoginResponeDto>(
                true,
                loginMessage,
                token
            ));
        }

        [HttpPost("regenerate-access")]
        public async Task<IActionResult> RegenerateAccessAsync([FromBody] RegenerateAccessTokenRequestDto token)
        {
            var accessToken = await _tokenService.RegenerateAccessTokenAsync(token);
            return Created(nameof(RegenerateAccessAsync), new ApiRespone<RegenerateAccessTokenResponeDto>(
                true,
                "Generate access token success",
                accessToken
            ));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequestDto token)
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);

            var nextUrl = Url.Action(nameof(LoginAsync));
            Response.Headers.Location = nextUrl;
            var refreshToken = await _authService.LogoutAsync(userIdInt, token);
            return Ok(new ApiRespone<LogoutResponeDto>(
                true,
                "Logout success!",
                refreshToken
            ));
        }

        [HttpPost("generate-otp")]
        [Authorize]
        public async Task<IActionResult> GenerateOTPAsync()
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);

            var nextUrl = Url.Action(nameof(LoginAsync));
            Response.Headers.Location = nextUrl;
            var result = await _authService.GenerateOTPAsync(userIdInt);
            return Accepted(new ApiRespone<GenerateOTPDto>(
                true,
                "Check your mail box!",
                result
            ));
        }

        [HttpPost("verify-otp")]
        [Authorize]
        public async Task<IActionResult> VerifyOtpAsync([FromBody] VerifyOtpRequestDto verifyOtpRequestDto)
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);

            var nextUrl = Url.Action(nameof(ChangePasswordAsync));
            Response.Headers.Location = nextUrl;
            var result = await _authService.VerifyOtpAsync(userIdInt, verifyOtpRequestDto);
            return Accepted(new ApiRespone<VerifyOtpResponeDto>(
                true,
                "OTP has been verified",
                result
            ));
        }


        [HttpPost("change-password")]
        [Authorize(Policy = "VerifiedOnly")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDto ChangePasswordRequestDto)
        {
            string userIdStr = User.Claims.FirstOrDefault(u => u.Type == "UserId").Value;
            int userIdInt = Convert.ToInt32(userIdStr);

            var nextUrl = Url.Action(nameof(LoginAsync));
            Response.Headers.Location = nextUrl;
            var result = await _authService.ChangePasswordAsync(userIdInt ,ChangePasswordRequestDto);
            return Ok(new ApiRespone<ChangePasswordResponeDto>(
                true,
                "Success change password!",
                result
            ));
        }
        
    }
}