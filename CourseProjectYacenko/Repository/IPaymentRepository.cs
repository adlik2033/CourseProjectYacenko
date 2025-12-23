using CourseProjectYacenko.Interfaces;
using CourseProjectYacenko.Models;

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IEnumerable<Payment>> GetPaymentsBySubscriberAsync(int subscriberId);
    Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalRevenueAsync();
    Task<Dictionary<string, decimal>> GetRevenueByPaymentMethodAsync();
}