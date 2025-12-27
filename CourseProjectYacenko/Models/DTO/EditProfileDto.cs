using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.DTO.User
{
    public class EditProfileDto
    {
        [Required(ErrorMessage = "ФИО обязательно")]
        [Display(Name = "ФИО")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [RegularExpression(@"^\+7\d{10}$", ErrorMessage = "Формат: +7XXXXXXXXXX")]
        [Display(Name = "Номер телефона")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Display(Name = "Адрес")]
        public string? Address { get; set; }

        [Display(Name = "Паспортные данные")]
        public string? PassportData { get; set; }
    }

    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Текущий пароль обязателен")]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "Новый пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль должен содержать минимум 6 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Подтверждение пароля обязательно")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; } = null!;
    }
}