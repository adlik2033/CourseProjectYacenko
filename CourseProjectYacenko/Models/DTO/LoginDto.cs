using System.ComponentModel.DataAnnotations;

namespace CourseProjectYacenko.Models
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Номер телефона обязателен")]
        [Phone(ErrorMessage = "Некорректный формат номера телефона")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Пароль обязателен")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}