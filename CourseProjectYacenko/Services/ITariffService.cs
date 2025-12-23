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
        Task<TariffDto> CreateTariffAsync(CreateTariffDto createDto);
        Task UpdateTariffAsync(int id, TariffDto updateDto);
        Task DeleteTariffAsync(int id);
        Task<bool> AssignTariffToUserAsync(int tariffId, int userId);
        Task<bool> RemoveTariffFromUserAsync(int tariffId, int userId);
        Task<IEnumerable<TariffDto>> FindMatchingTariffsAsync(int minutes, int internetGb, int sms);
        Task<IEnumerable<TariffDto>> GetTariffsWithServicesAsync();
    }
}