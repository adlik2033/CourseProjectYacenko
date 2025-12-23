using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public enum ServiceType
    {
        Internet,
        Calls,
        SMS,
        Entertainment,
        Security,
        Other
    }

    public enum BillingPeriod
    {
        Daily,
        Weekly,
        Monthly,
        OneTime
    }

    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название услуги обязательно")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;

        public ServiceType Type { get; set; }

        [Required(ErrorMessage = "Стоимость обязательна")]
        [Range(0, double.MaxValue, ErrorMessage = "Стоимость не может быть отрицательной")]
        public decimal Cost { get; set; }

        public BillingPeriod BillingPeriod { get; set; } = BillingPeriod.Monthly;

        // Внешний ключ
        public int? TariffId { get; set; }

        // Навигационные свойства
        public virtual Tariff Tariff { get; set; } = null!;
    }
}