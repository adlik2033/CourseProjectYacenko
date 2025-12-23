using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace CourseProjectYacenko.Models
{
    public class Subscriber
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "ФИО обязательно для заполнения")]
        [Display(Name = "ФИО")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "ФИО должно быть от 3 до 100 символов")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Display(Name = "Номер телефона")]
        [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Формат: +7XXXXXXXXXX")]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Хэш пароля")]
        public string PasswordHash { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Адрес")]
        [StringLength(200, ErrorMessage = "Адрес не должен превышать 200 символов")]
        public string Address { get; set; } = null!;

        [Display(Name = "Паспортные данные")]
        [StringLength(100, ErrorMessage = "Паспортные данные не должны превышать 100 символов")]
        public string PassportData { get; set; } = null!;

        [Display(Name = "Дата регистрации")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Display(Name = "Баланс")]
        [Range(0, double.MaxValue, ErrorMessage = "Баланс не может быть отрицательным")]
        public decimal Balance { get; set; } = 0;

        // Навигационные свойства
        public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}