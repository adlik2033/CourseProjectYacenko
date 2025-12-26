using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CourseProjectYacenko.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CourseProjectYacenko.Helpers
{
    public interface IJwtService
    {
        string GenerateToken(AppUser user);
        string GenerateRefreshToken();
    }

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["Jwt:Key"]
                ?? throw new ArgumentNullException("Jwt:Key", "JWT ключ не настроен в конфигурации");

            if (string.IsNullOrEmpty(_secretKey) || _secretKey.Length < 16)
            {
                throw new ArgumentException("JWT ключ должен быть не менее 16 символов");
            }
        }

        public string GenerateToken(AppUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.PhoneNumber),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FullName", user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenExpirationMinutes = _configuration.GetValue<int>("Jwt:TokenExpirationInMinutes", 60);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "MobileOperator",
                audience: _configuration["Jwt:Audience"] ?? "MobileOperatorClient",
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}