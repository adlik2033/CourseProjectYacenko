using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<bool> UpdateUserRoleAsync(int userId, string role);
        Task<bool> BlockUserAsync(int userId);
        Task<bool> UnblockUserAsync(int userId);
        Task<UserDto> GetUserProfileAsync(int userId);
        Task<bool> UpdateUserProfileAsync(int userId, UserDto updateDto);
        Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId);
        Task<IEnumerable<TariffDto>> GetUserTariffsAsync(int userId);
        Task<bool> AddBalanceAsync(int userId, decimal amount);
        Task<decimal> GetUserBalanceAsync(int userId);
    }
}