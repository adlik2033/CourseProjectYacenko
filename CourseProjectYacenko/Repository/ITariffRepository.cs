using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public interface ITariffRepository : IRepository<Tariff>
    {
        Task<IEnumerable<Tariff>> GetTariffsWithServicesAsync();
        Task<Tariff> GetTariffWithServicesAsync(int id);
        Task<IEnumerable<Tariff>> GetPopularTariffsAsync(int count);
        Task<bool> AssignTariffToUserAsync(int tariffId, int userId);
        Task<bool> RemoveTariffFromUserAsync(int tariffId, int userId);
        Task<IEnumerable<Tariff>> FindMatchingTariffsAsync(int minutes, int internetGb, int sms);
    }
}