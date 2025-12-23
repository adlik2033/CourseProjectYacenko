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
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название услуги обязательно")]
        [Display(Name = "Название")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов")]
        public string Name { get; set; } = null!;

        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string Description { get; set; } = null!;

        [Display(Name = "Тип услуги")]
        public ServiceType Type { get; set; }

        [Required(ErrorMessage = "Стоимость обязательна")]
        [Display(Name = "Стоимость")]
        [Range(0, double.MaxValue, ErrorMessage = "Стоимость не может быть отрицательной")]
        public decimal Cost { get; set; }

        [Display(Name = "Периодичность списания")]
        public BillingPeriod BillingPeriod { get; set; } = BillingPeriod.Monthly;

        // Внешний ключ
        [Display(Name = "Тариф")]
        public int? TariffId { get; set; }

        // Навигационные свойства
        [Display(Name = "Тариф")]
        public virtual Tariff Tariff { get; set; } = null!;
    }
}