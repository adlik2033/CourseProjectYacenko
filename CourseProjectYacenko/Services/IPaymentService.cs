using CourseProjectYacenko.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public interface IPaymentService
    {
        // Основные операции
        Task<PaymentDto> GetPaymentAsync(int id);
        Task<IEnumerable<PaymentDto>> GetPaymentsByUserAsync(int userId);
        Task<IEnumerable<PaymentDto>> GetRecentPaymentsAsync(int count = 10);
        Task<PaymentDto> CreatePaymentAsync(int userId, CreatePaymentDto createDto);
        Task<bool> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);

        // Для администраторов
        Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync();
        Task<IEnumerable<PaymentDto>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<PaymentDto>> GetFailedPaymentsAsync();

        // Статистика и отчеты
        Task<PaymentStatisticsDto> GetPaymentStatisticsAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<decimal> GetUserTotalPaidAsync(int userId);
        Task<Dictionary<string, decimal>> GetRevenueByPaymentMethodAsync();
        Task<Dictionary<string, decimal>> GetMonthlyRevenueAsync(int months = 12);
    }

    public class PaymentStatisticsDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalPayments { get; set; }
        public int SuccessfulPayments { get; set; }
        public int FailedPayments { get; set; }
        public decimal AveragePaymentAmount { get; set; }
        public Dictionary<string, decimal> RevenueByMethod { get; set; } = new();
        public Dictionary<string, decimal> MonthlyRevenue { get; set; } = new();
    }
}