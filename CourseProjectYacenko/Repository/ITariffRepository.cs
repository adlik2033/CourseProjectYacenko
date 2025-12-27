using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public interface ITariffRepository
    {
        Task<Tariff> GetByIdAsync(int id);
        Task<Tariff> GetByIdWithServicesAsync(int id);
        Task<List<Tariff>> GetAllAsync();
        Task<List<Tariff>> GetAllWithServicesAsync();
        Task<Tariff> AddAsync(Tariff tariff);
        Task<Tariff> UpdateAsync(Tariff tariff);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}