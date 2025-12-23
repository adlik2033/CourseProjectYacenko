using CourseProjectYacenko.Models;

namespace CourseProjectYacenko.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> RefreshTokenAsync(string token, string refreshToken);
        Task<bool> LogoutAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<UserDto> GetCurrentUserAsync(int userId);
    }
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
    }
}