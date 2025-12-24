using CourseProjectYacenko.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> GetPaymentAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByUserAsync(int userId);
        Task<PaymentDto> CreatePaymentAsync(int userId, decimal amount, string paymentMethod);
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetUserTotalPaidAsync(int userId);
    }
}