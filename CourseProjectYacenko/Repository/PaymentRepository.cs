using CourseProjectYacenko.Data;
using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(int userId)
        {
            return await _dbSet
                .Include(p => p.AppUser)
                .Where(p => p.AppUserId == userId)
                .OrderByDescending(p => p.PaymentDateTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(p => p.AppUser)
                .Where(p => p.PaymentDateTime >= startDate && p.PaymentDateTime <= endDate)
                .OrderByDescending(p => p.PaymentDateTime)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _dbSet
                .Where(p => p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount);
        }

        public async Task<decimal> GetUserTotalPaidAsync(int userId)
        {
            return await _dbSet
                .Where(p => p.AppUserId == userId && p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount);
        }

        public async Task<IEnumerable<Payment>> GetRecentPaymentsAsync(int count)
        {
            return await _dbSet
                .Include(p => p.AppUser)
                .Where(p => p.Status == PaymentStatus.Completed)
                .OrderByDescending(p => p.PaymentDateTime)
                .Take(count)
                .ToListAsync();
        }

        public async Task<Dictionary<string, decimal>> GetRevenueByPaymentMethodAsync()
        {
            return await _dbSet
                .Where(p => p.Status == PaymentStatus.Completed)
                .GroupBy(p => p.PaymentMethod)
                .Select(g => new { Method = g.Key.ToString(), Total = g.Sum(p => p.Amount) })
                .ToDictionaryAsync(x => x.Method, x => x.Total);
        }

        public async Task<Dictionary<string, decimal>> GetMonthlyRevenueAsync(int months = 12)
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);

            return await _dbSet
                .Where(p => p.Status == PaymentStatus.Completed && p.PaymentDateTime >= startDate)
                .GroupBy(p => new { Year = p.PaymentDateTime.Year, Month = p.PaymentDateTime.Month })
                .Select(g => new
                {
                    Key = $"{g.Key.Month:00}/{g.Key.Year}",
                    Total = g.Sum(p => p.Amount)
                })
                .ToDictionaryAsync(x => x.Key, x => x.Total);
        }

        public async Task<int> GetSuccessfulPaymentsCountAsync()
        {
            return await _dbSet
                .Where(p => p.Status == PaymentStatus.Completed)
                .CountAsync();
        }

        public async Task<decimal> GetAveragePaymentAmountAsync()
        {
            return await _dbSet
                .Where(p => p.Status == PaymentStatus.Completed)
                .AverageAsync(p => p.Amount);
        }

        public async Task<IEnumerable<Payment>> GetFailedPaymentsAsync()
        {
            return await _dbSet
                .Include(p => p.AppUser)
                .Where(p => p.Status == PaymentStatus.Failed)
                .OrderByDescending(p => p.PaymentDateTime)
                .ToListAsync();
        }

        public override async Task<Payment> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(p => p.AppUser)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}