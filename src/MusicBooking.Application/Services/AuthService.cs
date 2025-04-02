using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Win32;
using MusicBooking.Application.Interface;
using MusicBooking.Application.Util;
using MusicBooking.Domain;
using MusicBooking.Domain.Dtos;
using MusicBooking.Domain.Entites;
using MusicBooking.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;

namespace MusicBooking.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAppSessionContextRepository _appSessionContextRepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appsettings;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IOptions<AppSettings> appSettings,
            IUserRepository userRepository,
            IMapper mapper, ILogger<AuthService> logger, IAppSessionContextRepository appSessionContextRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _appsettings = appSettings.Value;
            _logger = logger;
            _appSessionContextRepository = appSessionContextRepository;
        }

        public async Task<AuthResponseDto> Register(RegisterUserDto registerDto)
        {
            try
            {
                var userCheck = await _userRepository.GetUserByEmailAsync(registerDto.Email);
                if (userCheck != null)
                {
                    _logger.LogInformation("User email already exists....");
                    return new AuthResponseDto();
                }

                var user = _mapper.Map<AppUser>(registerDto);
                user.CreatedAt = DateTime.UtcNow;
                user.Email = registerDto.Email;

                var hashdetails = HashingUtility.HashPassword(registerDto.Password, _appsettings);
                user.PasswordHash = hashdetails.HashedValue;
                user.Salt = hashdetails.Salt;
                user.CreatedAt = DateTime.UtcNow;
                user.IsOrganizer = registerDto.IsOrganizer;
               
                _logger.LogInformation($"Successfully created User {registerDto.Email}....");

                var resp = new AuthResponseDto
                {
                    Status = true,
                    Message = "User successfully created",
                    User = _mapper.Map<UserDto>(user)
                };
                return resp;
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Status = false,
                    Message = "Failed to create User due to error",
                    User = new UserDto()
                }; 
            }
        }

        public async Task<LoginViewModel> Login(LoginUserDto loginDto)
        {
            // Authenticate user
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogInformation("The User email was not found....");
                return new LoginViewModel() { Message = "The User email was not found...." };
            }

            if (!user.IsActive)
                return new LoginViewModel() { Message = "Your Account has been disabled please Contact Admin " };

            var passwordHasher = new PasswordHasher<AppUser>();
            var hashedPassword = HashingUtility.HashPassword(loginDto.Password, _appsettings, user.Salt);

            if (hashedPassword.HashedValue != user.PasswordHash)
            {
                _logger.LogInformation("Invalid Username or Password provided...");
                return new LoginViewModel() { Message = "Invalid Username or Password provided..." };
            }

            var token = AuthenticateUser(user);

            return token;
        }

        public async Task<UserDto> GetCurrentUser()
        {
            var userIdClaim = await _appSessionContextRepository.GetUserDetails();
            if (userIdClaim == null)
                return new UserDto();

            var user = await _userRepository.GetUserWithDetailsAsync(userIdClaim.UserId);
            if (user == null)
                return new UserDto();

            var resp = _mapper.Map<UserDto>(user);

            return resp;
        }

        private LoginViewModel AuthenticateUser(AppUser user)
        {
            try
            {
                var roleClaims = new List<Claim>();
                var claims = new List<Claim>();

                claims = new List<Claim>
                {
                    new(ClaimTypeHelper.Email, user.Email),
                    new(ClaimTypeHelper.UserId, user.Id.ToString()),
                    new(ClaimTypeHelper.PhoneNumber, user.PhoneNumber.ToString()),
                    new(ClaimTypeHelper.IsOrganizer, user.IsOrganizer.ToString()),
                };

                var jwtUserSecret = _appsettings.JwtSecret;
                var tokenExpireIn = GetTokenExpireIn();
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtUserSecret);
                var jwtTokenExpiration = DateTime.UtcNow.AddMinutes(tokenExpireIn);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = jwtTokenExpiration,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwt = tokenHandler.WriteToken(token);

                return new LoginViewModel
                {
                    Email = user.Email,
                    AccessToken = jwt,
                    JwtTokenExpiry = jwtTokenExpiration,
                    UserId = user.Id,
                    Message = "Successfully logged in User",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create jwt token for user with error: {ex.Message}");
                return new LoginViewModel()
                {
                    Message = $"Error : {ex.Message}"
                };
            }
        }

        private int GetTokenExpireIn()
        {
            var tokenLifeSpan = _appsettings.TokenLifeSpan;
            var tokenExpireIn = string.IsNullOrEmpty(tokenLifeSpan) ? 16 : int.Parse(tokenLifeSpan);
            return tokenExpireIn;
        }
    }
}
