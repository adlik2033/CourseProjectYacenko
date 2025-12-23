using System;

namespace CourseProjectYacenko.Models
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int AppUserId { get; set; }
        public string UserName { get; set; } = null!;
        public decimal Amount { get; set; }
        public DateTime PaymentDateTime { get; set; }
        public string PaymentMethod { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    public class CreatePaymentDto
    {
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = "Online";
    }
}