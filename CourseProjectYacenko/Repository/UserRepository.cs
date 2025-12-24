using CourseProjectYacenko.Data;
using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<AppUser> GetByPhoneAsync(string phoneNumber)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<AppUser> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser> GetUserWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(u => u.Tariffs)
                .Include(u => u.Payments)
                .Include(u => u.Applications)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<AppUser>> GetUsersWithTariffsAsync()
        {
            return await _dbSet
                .Include(u => u.Tariffs)
                .Where(u => u.Tariffs.Any())
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersWithLowBalanceAsync(decimal threshold)
        {
            return await _dbSet
                .Where(u => u.Balance < threshold)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            return await _dbSet.SumAsync(u => u.Balance);
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _dbSet.CountAsync(u => u.IsActive);
        }

        public override async Task<AppUser> GetByIdAsync(int id)
        {
            return await GetUserWithDetailsAsync(id);
        }
    }
}