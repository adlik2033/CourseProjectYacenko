using System;

namespace CourseProjectYacenko.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PassportData { get; set; } = null!;
        public DateTime RegistrationDate { get; set; }
        public decimal Balance { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class UserProfileDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PassportData { get; set; } = null!;
    }

    public class UpdateUserDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PassportData { get; set; } = null!;
    }
}