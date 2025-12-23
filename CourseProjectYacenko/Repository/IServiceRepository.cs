using CourseProjectYacenko.Interfaces;
using CourseProjectYacenko.Models;

public interface IServiceRepository : IRepository<Service>
{
    Task<IEnumerable<Service>> GetServicesByTypeAsync(string type);
    Task<IEnumerable<Service>> GetActiveServicesAsync();
    Task<decimal> CalculateMonthlyRevenueFromServicesAsync();
}