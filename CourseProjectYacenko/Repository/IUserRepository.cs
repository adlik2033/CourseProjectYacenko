using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public interface IUserRepository : IRepository<AppUser>
    {
        Task<AppUser> GetByPhoneAsync(string phoneNumber);
        Task<AppUser> GetByEmailAsync(string email);
        Task<AppUser> GetUserWithDetailsAsync(int id);
        Task<IEnumerable<AppUser>> GetUsersWithTariffsAsync();
        Task<IEnumerable<AppUser>> GetUsersWithLowBalanceAsync(decimal threshold);
        Task<decimal> GetTotalBalanceAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<bool> AddBalanceAsync(int userId, decimal amount);
    }
}