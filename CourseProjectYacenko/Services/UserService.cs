using CourseProjectYacenko.Data;
using CourseProjectYacenko.DTO;
using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public UserService(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        // Основные методы
        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<UserDto?> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                PassportData = user.PassportData,
                RegistrationDate = user.RegistrationDate,
                Balance = user.Balance,
                Role = user.Role,
                IsActive = user.IsActive
            };
        }

        public async Task<bool> UpdateUserAsync(int userId, EditProfileDto model)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            // Проверка уникальности телефона
            if (user.PhoneNumber != model.PhoneNumber)
            {
                var existingUser = await _userRepository.GetByPhoneAsync(model.PhoneNumber);
                if (existingUser != null && existingUser.Id != userId)
                    throw new InvalidOperationException("Пользователь с таким номером телефона уже существует");
            }

            // Проверка уникальности email
            if (user.Email != model.Email)
            {
                var existingUser = await _userRepository.GetByEmailAsync(model.Email);
                if (existingUser != null && existingUser.Id != userId)
                    throw new InvalidOperationException("Пользователь с таким email уже существует");
            }

            // Обновление данных пользователя
            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.Email = model.Email;
            user.Address = model.Address ?? user.Address;
            user.PassportData = model.PassportData ?? user.PassportData;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UserDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            // Обновляем только разрешенные поля
            user.FullName = updateDto.FullName;
            user.PhoneNumber = updateDto.PhoneNumber;
            user.Email = updateDto.Email;
            user.Address = updateDto.Address;
            user.PassportData = updateDto.PassportData;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        // Управление балансом
        public async Task<bool> UpdateBalanceAsync(int userId, decimal amount)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Balance += amount;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddBalanceAsync(int userId, decimal amount)
        {
            return await UpdateBalanceAsync(userId, amount);
        }

        public async Task<decimal> GetUserBalanceAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.Balance ?? 0;
        }

        // Управление статусом
        public async Task<bool> DeactivateUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = true;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BlockUserAsync(int userId)
        {
            return await DeactivateUserAsync(userId);
        }

        public async Task<bool> UnblockUserAsync(int userId)
        {
            return await ActivateUserAsync(userId);
        }

        // Управление ролями
        public async Task<bool> ChangeUserRoleAsync(int userId, string newRole)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Role = newRole;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string role)
        {
            return await ChangeUserRoleAsync(userId, role);
        }

        // История
        public async Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId)
        {
            var payments = await _context.Payments
                .Where(p => p.AppUserId == userId)
                .OrderByDescending(p => p.PaymentDateTime)
                .ToListAsync();

            return payments.Select(p => new PaymentDto
            {
                Id = p.Id,
                Amount = p.Amount,
                PaymentDateTime = p.PaymentDateTime,
                PaymentMethod = p.PaymentMethod.ToString(),
                Status = p.Status.ToString()
            });
        }

        public async Task<IEnumerable<TariffDto>> GetUserTariffsAsync(int userId)
        {
            var user = await _userRepository.GetByIdWithTariffsAsync(userId);
            if (user == null) return Enumerable.Empty<TariffDto>();

            return user.Tariffs.Select(t => new TariffDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                MonthlyFee = t.MonthlyFee,
                InternetTrafficGB = t.InternetTrafficGB,
                MinutesCount = t.MinutesCount,
                SmsCount = t.SmsCount,
                ConnectedServices = t.ConnectedServices.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Cost = s.Cost,
                    Description = s.Description
                }).ToList()
            });
        }

        // Список пользователей
        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersDtoAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                PhoneNumber = u.PhoneNumber,
                Email = u.Email,
                Address = u.Address,
                PassportData = u.PassportData,
                RegistrationDate = u.RegistrationDate,
                Balance = u.Balance,
                Role = u.Role,
                IsActive = u.IsActive
            });
        }

        // Прочие методы
        public async Task<bool> UpdateLastLoginAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.LastLoginDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}