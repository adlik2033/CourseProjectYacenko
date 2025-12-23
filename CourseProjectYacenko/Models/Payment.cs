using System;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public enum PaymentMethod
    {
        CreditCard,
        BankTransfer,
        Cash,
        Online,
        MobilePayment
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Сумма платежа обязательна")]
        [Display(Name = "Сумма")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Сумма должна быть больше 0")]
        public decimal Amount { get; set; }

        [Display(Name = "Дата и время")]
        public DateTime PaymentDateTime { get; set; } = DateTime.Now;

        [Display(Name = "Способ оплаты")]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Статус")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Внешний ключ
        [Required]
        [Display(Name = "Абонент")]
        public int SubscriberId { get; set; }

        // Навигационные свойства
        [Display(Name = "Абонент")]
        public virtual Subscriber Subscriber { get; set; } = null!;
    }
}