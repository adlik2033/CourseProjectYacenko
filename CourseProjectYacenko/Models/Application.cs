using System;
using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public enum ApplicationType
    {
        NewConnection,
        TariffChange,
        ServiceActivation,
        ServiceDeactivation,
        PersonalDataChange,
        NumberPorting,
        Complaint,
        Other
    }

    public enum ApplicationStatus
    {
        New,
        InProgress,
        Completed,
        Rejected,
        OnHold
    }

    public class Application
    {
        public int Id { get; set; }

        [Required]
        public int AppUserId { get; set; }

        [Required]
        public ApplicationType Type { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public DateTime? ProcessingDate { get; set; }

        public ApplicationStatus Status { get; set; } = ApplicationStatus.New;

        [MaxLength(1000)]
        public string Comment { get; set; } = null!;

        // Навигационные свойства
        public virtual AppUser AppUser { get; set; } = null!;
    }
}