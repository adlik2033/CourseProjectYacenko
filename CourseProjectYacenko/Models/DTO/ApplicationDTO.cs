using System;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.DTO
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public int SubscriberId { get; set; }
        public string SubscriberName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public string Status { get; set; } = null!;
        public string Comment { get; set; } = null!;
    }

    public class CreateApplicationDto
    {
        [Required]
        public int SubscriberId { get; set; }

        [Required]
        public string Type { get; set; } = null!;

        public string Comment { get; set; } = null!;
    }

    public class UpdateApplicationStatusDto
    {
        [Required]
        public string Status { get; set; } = null!;

        public string AdminComment { get; set; } = null!;
    }

    public class ApplicationStatisticsDto
    {
        public int TotalApplications { get; set; }
        public int NewApplications { get; set; }
        public int CompletedApplications { get; set; }
        public Dictionary<string, int> ApplicationsByType { get; set; } = new();
        public double AverageProcessingTimeHours { get; set; }
    }
}