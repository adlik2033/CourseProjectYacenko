namespace CourseProjectYacenko.Helpers
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty; // Значение по умолчанию
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int TokenExpirationInMinutes { get; set; } = 60;
        public int RefreshTokenExpirationInDays { get; set; } = 7;
    }
}