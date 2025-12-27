using CourseProjectYacenko.Models;

namespace CourseProjectYacenko.Repository
{
    public interface ITariffRepository
    {
        Task<Tariff?> GetByIdAsync(int id);
        Task<List<Tariff>> GetAllAsync();
        Task AddAsync(Tariff tariff);
        Task UpdateAsync(Tariff tariff);
        Task DeleteAsync(int id);
        Task<bool> SaveChangesAsync();
    }
}