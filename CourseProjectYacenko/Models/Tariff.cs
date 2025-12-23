using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public class Tariff
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название тарифа обязательно")]
        [MaxLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Ежемесячная плата обязательна")]
        [Range(0, double.MaxValue, ErrorMessage = "Плата не может быть отрицательной")]
        public decimal MonthlyFee { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Объем трафика не может быть отрицательным")]
        public int InternetTrafficGB { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество минут не может быть отрицательным")]
        public int MinutesCount { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Количество SMS не может быть отрицательным")]
        public int SmsCount { get; set; }

        // Внешний ключ
        public int? AppUserId { get; set; }

        // Навигационные свойства
        public virtual AppUser AppUser { get; set; } = null!;
        public virtual ICollection<Service> ConnectedServices { get; set; } = new List<Service>();
    }
}