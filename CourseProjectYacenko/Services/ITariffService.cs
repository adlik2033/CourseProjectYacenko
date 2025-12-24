using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public interface ITariffService
    {
        Task<TariffDto> GetTariffAsync(int id);
        Task<IEnumerable<TariffDto>> GetAllTariffsAsync();
        Task<IEnumerable<TariffDto>> GetPopularTariffsAsync(int count);
        Task<bool> AssignTariffToUserAsync(int tariffId, int userId);
        Task<bool> RemoveTariffFromUserAsync(int tariffId, int userId);
        Task<IEnumerable<TariffDto>> FindMatchingTariffsAsync(int minutes, int internetGb, int sms);
    }
}