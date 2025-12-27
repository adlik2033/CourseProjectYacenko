using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;

namespace CourseProjectYacenko.Services
{
    public interface IUserService
    {
        // Основные методы
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<UserDto?> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, EditProfileDto model);
        Task<bool> UpdateUserProfileAsync(int userId, UserDto updateDto);

        // Управление балансом
        Task<bool> UpdateBalanceAsync(int userId, decimal amount);
        Task<bool> AddBalanceAsync(int userId, decimal amount);
        Task<decimal> GetUserBalanceAsync(int userId);

        // Управление статусом
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);
        Task<bool> BlockUserAsync(int userId);
        Task<bool> UnblockUserAsync(int userId);

        // Управление ролями
        Task<bool> ChangeUserRoleAsync(int userId, string newRole);
        Task<bool> UpdateUserRoleAsync(int userId, string role);

        // История
        Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId);
        Task<IEnumerable<TariffDto>> GetUserTariffsAsync(int userId);

        // Список пользователей
        Task<List<AppUser>> GetAllUsersAsync();
        Task<IEnumerable<UserDto>> GetAllUsersDtoAsync();

        // Прочие методы
        Task<bool> UpdateLastLoginAsync(int userId);
    }
}