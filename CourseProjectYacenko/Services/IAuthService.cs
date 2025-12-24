using CourseProjectYacenko.Models;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<bool> LogoutAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<UserDto> GetCurrentUserAsync(int userId);
    }
}