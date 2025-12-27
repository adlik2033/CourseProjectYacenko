using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public class Tariff
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название тарифа обязательно")]
        [MaxLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        [Display(Name = "Название тарифа")]
        public string Name { get; set; } = null!;

        [MaxLength(500, ErrorMessage = "Описание не должно превышать 500 символов")]
        [Display(Name = "Описание")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Ежемесячная плата обязательна")]
        [Range(0, double.MaxValue, ErrorMessage = "Плата не может быть отрицательной")]
        [Display(Name = "Ежемесячная плата")]
        public decimal MonthlyFee { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Объем трафика не может быть отрицательным")]
        [Display(Name = "Интернет трафик (ГБ)")]
        public int InternetTrafficGB { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество минут не может быть отрицательным")]
        [Display(Name = "Количество минут")]
        public int MinutesCount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество SMS не может быть отрицательным")]
        [Display(Name = "Количество SMS")]
        public int SmsCount { get; set; }

        // Навигационные свойства
        [Display(Name = "Подключенные услуги")]
        public virtual ICollection<Service> ConnectedServices { get; set; } = new List<Service>();

        // Связь многие-ко-многим с AppUser
        public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();
    }
}