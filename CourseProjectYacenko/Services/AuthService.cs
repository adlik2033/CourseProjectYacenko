using AutoMapper;
using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Helpers;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            IJwtService jwtService,
            IUserRepository userRepository,
            IMapper mapper,
            IOptions<JwtSettings> jwtSettings)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByPhoneAsync(loginDto.PhoneNumber);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Неверный номер телефона или пароль");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("Аккаунт заблокирован");

            user.LastLoginDate = DateTime.UtcNow;
            user.RefreshToken = _jwtService.GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = await _userRepository.GetByPhoneAsync(registerDto.PhoneNumber);
            if (existingUser != null)
                throw new InvalidOperationException("Пользователь с таким номером телефона уже существует");

            existingUser = await _userRepository.GetByEmailAsync(registerDto.Email);
            if (existingUser != null)
                throw new InvalidOperationException("Пользователь с таким email уже существует");

            var user = new AppUser
            {
                FullName = registerDto.FullName,
                PhoneNumber = registerDto.PhoneNumber,
                Email = registerDto.Email,
                Address = registerDto.Address,
                PassportData = registerDto.PassportData,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                RegistrationDate = DateTime.UtcNow,
                Role = "User",
                IsActive = true,
                Balance = 0
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                User = _mapper.Map<UserDto>(user)
            };
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                return false;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<UserDto> GetCurrentUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

    }
}