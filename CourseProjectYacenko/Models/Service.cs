using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public enum ServiceType { Internet, Calls, SMS, Entertainment, Security, Other }
    public enum BillingPeriod { Daily, Weekly, Monthly, OneTime }

    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название услуги обязательно")]
        [MaxLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        [Display(Name = "Название услуги")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = null!;

        [Display(Name = "Тип услуги")]
        public ServiceType Type { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Стоимость не может быть отрицательной")]
        [Display(Name = "Стоимость")]
        public decimal Cost { get; set; }

        [Display(Name = "Период оплаты")]
        public BillingPeriod BillingPeriod { get; set; } = BillingPeriod.Monthly;

        // Внешний ключ - nullable!
        [Display(Name = "ID тарифа")]
        public int? TariffId { get; set; }

        // Навигационные свойства
        [Display(Name = "Тариф")]
        public virtual Tariff? Tariff { get; set; }
    }
}