using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public class Tariff
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Название тарифа обязательно")]
        [Display(Name = "Название")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Название должно быть от 3 до 50 символов")]
        public string Name { get; set; } = null!;

        [Display(Name = "Описание")]
        [StringLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Ежемесячная плата обязательна")]
        [Display(Name = "Ежемесячная плата")]
        [Range(0, double.MaxValue, ErrorMessage = "Плата не может быть отрицательной")]
        public decimal MonthlyFee { get; set; }

        [Display(Name = "Интернет трафик (ГБ)")]
        [Range(0, int.MaxValue, ErrorMessage = "Объем трафика не может быть отрицательным")]
        public int InternetTrafficGB { get; set; }

        [Display(Name = "Количество минут")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество минут не может быть отрицательным")]
        public int MinutesCount { get; set; }

        [Display(Name = "Количество SMS")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество SMS не может быть отрицательным")]
        public int SmsCount { get; set; }

        // Внешний ключ
        [Display(Name = "Абонент")]
        public int? SubscriberId { get; set; }

        // Навигационные свойства
        [Display(Name = "Абонент")]
        public virtual Subscriber Subscriber { get; set; } = null!;

        public virtual ICollection<Service> ConnectedServices { get; set; } = new List<Service>();
    }
}