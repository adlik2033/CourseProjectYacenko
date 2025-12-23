namespace CourseProjectYacenko.DTO
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Cost { get; set; }
        public string BillingPeriod { get; set; } = null!;
        public int? TariffId { get; set; }
    }

    public class CreateServiceDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Type { get; set; } = null!;
        public decimal Cost { get; set; }
        public string BillingPeriod { get; set; } = null!;
    }

    public class ServiceUsageDto
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = null!;
        public int SubscriberCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}