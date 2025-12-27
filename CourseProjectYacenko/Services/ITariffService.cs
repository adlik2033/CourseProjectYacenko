using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;

namespace CourseProjectYacenko.Services
{
    public interface ITariffService
    {
        Task<List<TariffDto>> GetAllTariffsAsync();
        Task<TariffDto?> GetTariffAsync(int id);
        Task<List<TariffDto>> GetUserTariffsAsync(int userId);
        Task<bool> UnsubscribeTariffAsync(int userId, int tariffId);
        Task<bool> SubscribeTariffAsync(int userId, int tariffId);

        // Добавьте эти методы для совместимости с контроллером
        Task<bool> RemoveTariffFromUserAsync(int userId, int tariffId);
        Task<bool> AssignTariffToUserAsync(int userId, int tariffId);
    }
}