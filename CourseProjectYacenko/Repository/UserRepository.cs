using CourseProjectYacenko.Data;
using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectYacenko.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser?> GetByIdWithTariffsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Tariffs)
                    .ThenInclude(t => t.ConnectedServices)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<AppUser?> GetByPhoneAsync(string phoneNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }

        public async Task<AppUser?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task UpdateAsync(AppUser user)
        {
            _context.Users.Update(user);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await GetByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}