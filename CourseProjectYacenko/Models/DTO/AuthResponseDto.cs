using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;
using System;

namespace CourseProjectYacenko.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public DateTime Expiration { get; set; }
        public UserDto User { get; set; } = null!;
    }
}