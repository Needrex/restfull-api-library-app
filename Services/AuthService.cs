using RestApiApp.InterfaceServices;
using RestApiApp.Dtos;
using RestApiApp.InterfaceRepositories;
using RestApiApp.Utils;
using RestApiApp.Exceptions;
using RestApiApp.Models;
using RestApiApp.Repositories;
using MailKit.Net.Smtp;
using MimeKit;
using RestApiApp.Configurations;
using Microsoft.Extensions.Options;
using AutoMapper;

namespace RestApiApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly EmailSettings _emailSettings;
        private readonly IRazorViewRenderer _razorViewRenderer;
        private readonly FrontendOptions _frontendOptions;
        private readonly IUserOtpsRepository _userOtps;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository,
                            ITokenService tokenService,
                            IRefreshTokenRepository refreshTokenRepository,
                            IOptions<EmailSettings> emailSettings,
                            IRazorViewRenderer razorViewRenderer,
                            IOptions<FrontendOptions> frontendOptions,
                            IUserOtpsRepository userOtps,
                            IMapper mapper,
                            ILogger<AuthService> logger)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _emailSettings = emailSettings.Value;
            _razorViewRenderer = razorViewRenderer;
            _frontendOptions = frontendOptions.Value;
            _userOtps = userOtps;
            _mapper = mapper;
            _logger = logger;
        }


        public async Task<RegisterResponeDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var userCheck = await _userRepository.GetAsyc(
                                user => user.Username == registerRequestDto.Username
                                && user.Email == registerRequestDto.Email);
                                
            if (userCheck != null) throw new ConflictException("Username or Email already use!");

            var userEntity = _mapper.Map<Users>(registerRequestDto);
            userEntity.Password = PasswordHasher.HashPassword(registerRequestDto.Password);

            var createUser = await _userRepository.CreateAsyc(userEntity);
            
            var registerResponeDto = _mapper.Map<RegisterResponeDto>(createUser);
            return registerResponeDto;
        }


        public async Task<LoginResponeDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var checkUser = await _userRepository.GetAsyc(
                            user => user.Username == loginRequestDto.Username
                            && user.Email == loginRequestDto.Email)
                            ?? throw new NotFoundException("Password or username does not match!");

            if (!PasswordHasher.VerifyPassword(loginRequestDto.Password, checkUser.Password))
            {
                throw new NotFoundException("Password or username does not match!");
            }

            await _userRepository.UpdateLastLoginAsync(checkUser);

            var dataAccessToken = _mapper.Map<AccessTokenRequestDto>(checkUser);
            if (checkUser.RefreshToken == null)
            {
                string refreshTokenHash = PasswordHasher.HashPassword(_tokenService.GenerateRefreshTokenAsync());
                var newRefreshToken = new RefreshToken
                {
                    Token = refreshTokenHash,
                    User = checkUser,
                    UserId = checkUser.Id
                };
                var createRefreshToken = await _refreshTokenRepository.AddAsyc(newRefreshToken);

                var loginResponeDto = _mapper.Map<LoginResponeDto>(checkUser);
                loginResponeDto.AccessToken = _tokenService.GenerateAccessTokenAsync(dataAccessToken);
                loginResponeDto.RefreshToken = createRefreshToken.Token;
                loginResponeDto.StatusAuthentication = false;

                return loginResponeDto;
            }


            var dateNow = DateTime.Now;
            bool refreshTokenExpired = dateNow > checkUser.RefreshToken.ExpiresAt;
            if (refreshTokenExpired || checkUser.RefreshToken.IsRevoked)
            {
                string refreshTokenHash = PasswordHasher.HashPassword(_tokenService.GenerateRefreshTokenAsync());
                var newRefreshToken = new RefreshToken
                {
                    Token = refreshTokenHash,
                    IsRevoked = false,
                    RevokedAt = null
                };
                var updateRefreshToken = await _refreshTokenRepository.UpdateRevokedStatusAsync(checkUser.RefreshToken.Id, newRefreshToken);


                var loginResponeDto = _mapper.Map<LoginResponeDto>(checkUser);
                loginResponeDto.AccessToken = _tokenService.GenerateAccessTokenAsync(dataAccessToken);
                loginResponeDto.RefreshToken = updateRefreshToken.Token;
                loginResponeDto.StatusAuthentication = false;
                
                return loginResponeDto;
            }
            else
            {
                var loginResponeDto = _mapper.Map<LoginResponeDto>(checkUser);
                loginResponeDto.AccessToken = _tokenService.GenerateAccessTokenAsync(dataAccessToken);
                loginResponeDto.StatusAuthentication = true;
                
                return loginResponeDto;
            }
        }


        public async Task<LogoutResponeDto> LogoutAsync(int userId, LogoutRequestDto logoutRequestDto)
        {
            var checkUser = await _userRepository.GetAsyc(user => user.Id == userId)
                        ?? throw new NotFoundException("User not found!");

            if (checkUser.RefreshToken.IsRevoked == true)
            {
                var logoutRespone = _mapper.Map<LogoutResponeDto>(checkUser);
                return logoutRespone;
            }
            else
            {
                var refreshTokenEntity = checkUser.RefreshToken;
                refreshTokenEntity.IsRevoked = true;
                refreshTokenEntity.RevokedAt = DateTime.Now;

                await _refreshTokenRepository.UpdateAsync(checkUser.RefreshToken.Id, refreshTokenEntity);

                var logoutRespone = _mapper.Map<LogoutResponeDto>(checkUser);
                return logoutRespone;
            }
        }


        public async Task<GenerateOTPDto> GenerateOTPAsync(int userId)
        {
            var checkUser = await _userRepository.GetAsyc(user => user.Id == userId)
                            ?? throw new NotFoundException("User not found!");

            var checkOtp = await _userOtps.GetAsyc(o => o.UserId == checkUser.Id && o.IsUsed == false);
            if (checkOtp != null)
            {
                throw new ConflictException("OTP code already exists!");
            }

            var random = new Random();
            string codeOtp = random.Next(100000, 999999).ToString();
            string codeOtpHash = PasswordHasher.HashPassword(codeOtp);

            var userOtpEntity = new UserOtps
            {
                OtpCode = codeOtpHash,
                OtpType = OtpType.PasswordReset,
                UserId = checkUser.Id
            };

            var AddAsycCodeOtp = await _userOtps.AddAsyc(userOtpEntity);
            var resetUrl = $"{_frontendOptions.BaseUrl}/reset-passowrd?token={codeOtp}";

            var emailModel = new EmailContent
            {
                EmailRecipient = checkUser.Email,
                JoinDate = checkUser.CreatedAt,
                VerifyLink = resetUrl
            };

            const string FILENAME = "EmailTemplate.cshtml";
            const string FOLDERNAME = "Templates";

            string filePathEmailBody = Path.Combine(Directory.GetCurrentDirectory(), FOLDERNAME, FILENAME);
            if (!File.Exists(filePathEmailBody))
            {
                throw new NotFoundException("File view not found!");
            }

            string htmlBodyEmail = await _razorViewRenderer.RenderViewAsync(FILENAME, emailModel);
            await SendEmailAsync(checkUser.Email, "Reset Password", htmlBodyEmail);

            var generateOTPDto = new GenerateOTPDto
            {
                Username = checkUser.Username,
                Email = checkUser.Email
            };
            return generateOTPDto;
        }


        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }


        public async Task<VerifyOtpResponeDto> VerifyOtpAsync(int userId, VerifyOtpRequestDto verifyOtpRequestDto)
        {
            var checkOtp = await _userOtps.GetAsyc(o => o.UserId == userId
                                                && o.IsUsed == false)
                            ?? throw new NotFoundException("OTP not valid!");

            var otpExpired = DateTime.Now > checkOtp.ExpiredAt;
            if (otpExpired)
            {
                throw new ConflictException("OTP has expired!");
            }

            if (!PasswordHasher.VerifyPassword(verifyOtpRequestDto.Otp, checkOtp.OtpCode))
            {
                throw new ConflictException("OTP not valid!");
            }

            var accessTokenDto = new AccessTokenRequestDto
            {
                UserId = userId,
                Email = checkOtp.Users.Email,
                Username = checkOtp.Users.Username
            };
            var generateAccessToken = _tokenService.GenerateAccessTokenAsync(accessTokenDto, true);


            var userOtpEntity = new UserOtps
            {
                IsUsed = true
            };
            await _userOtps.UpdateOTPStatusAsyc(checkOtp.Id, userOtpEntity);


            var verifyOtpResponeDto = new VerifyOtpResponeDto
            {
                AccessToken = generateAccessToken
            };
            return verifyOtpResponeDto;

        }


        public async Task<ChangePasswordResponeDto> ChangePasswordAsync(int userId, ChangePasswordRequestDto changePasswordRequestDto)
        {
            var newPasswordHash = PasswordHasher.HashPassword(changePasswordRequestDto.NewPassword);
            var userEntity = new Users
            {
                Password = newPasswordHash
            };
            var userUpdate = await _userRepository.UpdateAsync(userId, userEntity)
                            ?? throw new NotFoundException("User not found!");


            var changePasswordResponeDto = new ChangePasswordResponeDto
            {
                Username = userUpdate.Username,
                Email = userUpdate.Email
            };
            return changePasswordResponeDto;
        }
    }
}