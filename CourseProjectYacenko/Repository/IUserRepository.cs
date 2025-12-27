using CourseProjectYacenko.Models;

namespace CourseProjectYacenko.Repository
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByIdAsync(int id);
        Task<AppUser?> GetByIdWithTariffsAsync(int id); 
        Task<AppUser?> GetByPhoneAsync(string phoneNumber);
        Task<AppUser?> GetByEmailAsync(string email);
        Task AddAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();

        Task<List<AppUser>> GetAllAsync();
    }
}