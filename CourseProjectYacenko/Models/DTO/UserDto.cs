using System;

namespace CourseProjectYacenko.DTO.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PassportData { get; set; }
        public DateTime RegistrationDate { get; set; }
        public decimal Balance { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateUserDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PassportData { get; set; }
    }
}