using CourseProjectYacenko.Interfaces;
using CourseProjectYacenko.Models;

public interface ITariffRepository : IRepository<Tariff>
{
    Task<IEnumerable<Tariff>> GetTariffsWithServicesAsync();
    Task<Tariff> GetTariffWithServicesAsync(int id);
    Task<IEnumerable<Tariff>> GetPopularTariffsAsync(int count);
    Task<bool> AssignTariffToSubscriberAsync(int tariffId, int subscriberId);
    Task<bool> RemoveTariffFromSubscriberAsync(int tariffId, int subscriberId);
}