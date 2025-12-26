using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "ФИО обязательно")]
        [MaxLength(100, ErrorMessage = "ФИО не должно превышать 100 символов")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [MaxLength(20, ErrorMessage = "Номер телефона не должен превышать 20 символов")]
        [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Формат: +7XXXXXXXXXX")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [MaxLength(100)]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [Display(Name = "Хэш пароля")]
        public string PasswordHash { get; set; } = null!;

        [MaxLength(200, ErrorMessage = "Адрес не должен превышать 200 символов")]
        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [MaxLength(100, ErrorMessage = "Паспортные данные не должны превышать 100 символов")]
        [Display(Name = "Паспортные данные")]
        public string? PassportData { get; set; }

        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue, ErrorMessage = "Баланс не может быть отрицательным")]
        [Display(Name = "Баланс")]
        public decimal Balance { get; set; } = 0;

        [Display(Name = "Роль")]
        public string Role { get; set; } = "User";

        [Display(Name = "Refresh Token")]
        public string? RefreshToken { get; set; }

        [Display(Name = "Время истечения Refresh Token")]
        public DateTime? RefreshTokenExpiryTime { get; set; }

        [Display(Name = "Дата последнего входа")]
        public DateTime? LastLoginDate { get; set; }

        [Display(Name = "Активен")]
        public bool IsActive { get; set; } = true;

        // Навигационные свойства
        public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}