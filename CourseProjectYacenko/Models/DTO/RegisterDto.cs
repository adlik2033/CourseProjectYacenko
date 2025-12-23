using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "ФИО обязательно")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Phone(ErrorMessage = "Некорректный формат номера телефона")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль должен быть не менее 6 символов")]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; } = null!;

        public string Address { get; set; } = null!;
        public string PassportData { get; set; } = null!;
    }
}