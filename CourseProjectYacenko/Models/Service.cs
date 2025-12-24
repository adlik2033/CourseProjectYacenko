using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public enum ServiceType { Internet, Calls, SMS, Entertainment, Security, Other }
    public enum BillingPeriod { Daily, Weekly, Monthly, OneTime }

    public class Service
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public ServiceType Type { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        public BillingPeriod BillingPeriod { get; set; } = BillingPeriod.Monthly;

        // Внешний ключ
        public int? TariffId { get; set; }

        // Навигационные свойства
        public virtual Tariff Tariff { get; set; }
    }
}