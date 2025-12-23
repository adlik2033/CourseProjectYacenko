using CourseProjectYacenko.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId);
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetUserTotalPaidAsync(int userId);
    }
}