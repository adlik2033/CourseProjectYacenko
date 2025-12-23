using System;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.DTO
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class CreatePaymentDto
    {
        [Required]
        public int SubscriberId { get; set; }

        [Range(0.01, 100000)]
        public decimal Amount { get; set; }

        public string PaymentMethod { get; set; } = "Online";
    }

    public class PaymentStatisticsDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal AveragePayment { get; set; }
        public int PaymentsCount { get; set; }
        public Dictionary<string, decimal> RevenueByMethod { get; set; } = new();
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; } = new();
    }

    public class MonthlyRevenueDto
    {
        public string Month { get; set; } = null!;
        public decimal Revenue { get; set; }
    }
}