using System;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.DTO
{
    public class SubscriberDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Формат: +7XXXXXXXXXX")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; } = null!;

        public string Address { get; set; } = null!;
        public string PassportData { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
        public decimal Balance { get; set; }
    }

    public class CreateSubscriberDto
    {
        [Required(ErrorMessage = "ФИО обязательно")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Формат: +7XXXXXXXXXX")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PassportData { get; set; } = null!;
        public decimal InitialBalance { get; set; } = 0;
    }

    public class UpdateSubscriberDto
    {
        [Required(ErrorMessage = "ФИО обязательно")]
        public string FullName { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PassportData { get; set; } = null!;
    }

    public class SubscriberBalanceDto
    {
        public int SubscriberId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public decimal Balance { get; set; }
        public DateTime LastPaymentDate { get; set; }
    }

    public class SubscriberWithTariffsDto : SubscriberDto
    {
        public List<TariffDto> Tariffs { get; set; } = new();
        public int ActiveTariffsCount { get; set; }
    }
}