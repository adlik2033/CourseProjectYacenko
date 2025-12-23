using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public interface IUserService
    {
        // Для администраторов
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<bool> UpdateUserRoleAsync(int userId, string role);
        Task<bool> BlockUserAsync(int userId);
        Task<bool> UnblockUserAsync(int userId);

        // Для авторизованных пользователей
        Task<UserDto> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto updateDto);
        Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId);
        Task<IEnumerable<TariffDto>> GetUserTariffsAsync(int userId);
        Task<bool> AddBalanceAsync(int userId, decimal amount);
        Task<decimal> GetUserBalanceAsync(int userId);

        // Статистика
        Task<Dictionary<string, int>> GetUserStatisticsAsync();
    }
}