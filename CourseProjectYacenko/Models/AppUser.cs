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
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [MaxLength(20, ErrorMessage = "Номер телефона не должен превышать 20 символов")]
        [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Формат: +7XXXXXXXXXX")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [MaxLength(100)]
        public string Email { get; set; } = null!;

        [Required]
        public string PasswordHash { get; set; } = null!;

        [MaxLength(200)]
        public string Address { get; set; } = null!;

        [MaxLength(100)]
        public string PassportData { get; set; } = null!;

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        
        [Range(0, double.MaxValue, ErrorMessage = "Баланс не может быть отрицательным")]
        public decimal Balance { get; set; } = 0;

        public string Role { get; set; } = "User";
        public string RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; } = true;

        // Навигационные свойства
        public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}