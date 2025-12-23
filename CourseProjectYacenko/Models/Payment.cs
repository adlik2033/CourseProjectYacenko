using System;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Cash,
        Online,
        MobilePayment
    }

    public class Payment
    {
        public int Id { get; set; }

        [Required]
        public int AppUserId { get; set; }

        [Required(ErrorMessage = "Сумма обязательна")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
        public decimal Amount { get; set; }

        public DateTime PaymentDateTime { get; set; } = DateTime.UtcNow;

        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Online;

        public PaymentStatus Status { get; set; } = PaymentStatus.Completed;

        // Навигационные свойства
        public virtual AppUser AppUser { get; set; } = null!;
    }
}