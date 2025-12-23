using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.DTO
{
    public class TariffDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название тарифа обязательно")]
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        [Range(0, double.MaxValue, ErrorMessage = "Плата не может быть отрицательной")]
        public decimal MonthlyFee { get; set; }

        [Range(0, int.MaxValue)]
        public int InternetTrafficGB { get; set; }

        [Range(0, int.MaxValue)]
        public int MinutesCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SmsCount { get; set; }

        public int? SubscriberId { get; set; }
        public List<ServiceDto> ConnectedServices { get; set; } = new();
    }

    public class CreateTariffDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        [Range(0.01, 10000)]
        public decimal MonthlyFee { get; set; }

        public int InternetTrafficGB { get; set; }
        public int MinutesCount { get; set; }
        public int SmsCount { get; set; }
        public List<int> ServiceIds { get; set; } = new();
    }

    public class AssignTariffDto
    {
        [Required]
        public int SubscriberId { get; set; }

        [Required]
        public int TariffId { get; set; }
    }

    public class TariffComparisonDto
    {
        public TariffDto BaseTariff { get; set; } = null!;
        public TariffDto ComparedTariff { get; set; } = null!;
        public List<string> Advantages { get; set; } = new();
        public decimal PriceDifference { get; set; }
    }
}