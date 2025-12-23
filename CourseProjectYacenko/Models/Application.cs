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
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Тип заявки")]
        public ApplicationType Type { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreationDate { get; set; } = DateTime.Now;

        [Display(Name = "Дата обработки")]
        public DateTime? ProcessingDate { get; set; }

        [Display(Name = "Статус")]
        public ApplicationStatus Status { get; set; } = ApplicationStatus.New;

        [Display(Name = "Комментарий")]
        [StringLength(1000, ErrorMessage = "Комментарий не должен превышать 1000 символов")]
        public string Comment { get; set; }

        // Внешний ключ
        [Required]
        [Display(Name = "Абонент")]
        public int SubscriberId { get; set; }

        // Навигационные свойства
        [Display(Name = "Абонент")]
        public virtual Subscriber Subscriber { get; set; } = null!;
    }
}