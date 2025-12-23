using System.Collections.Generic;

namespace CourseProjectYacenko.Models
{
    public class TariffDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal MonthlyFee { get; set; }
        public int InternetTrafficGB { get; set; }
        public int MinutesCount { get; set; }
        public int SmsCount { get; set; }
        public int? AppUserId { get; set; }
        public List<ServiceDto> ConnectedServices { get; set; } = new();
    }

    public class CreateTariffDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal MonthlyFee { get; set; }
        public int InternetTrafficGB { get; set; }
        public int MinutesCount { get; set; }
        public int SmsCount { get; set; }
    }

    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Cost { get; set; }
        public string BillingPeriod { get; set; } = null!;
    }
}