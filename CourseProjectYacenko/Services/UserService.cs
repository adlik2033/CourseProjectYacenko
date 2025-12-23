using AutoMapper;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ITariffRepository _tariffRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IPaymentRepository paymentRepository,
            ITariffRepository tariffRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _paymentRepository = paymentRepository;
            _tariffRepository = tariffRepository;
            _mapper = mapper;
        }

        // Методы для администраторов
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, string role)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Role = role;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> BlockUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = false;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UnblockUserAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.IsActive = true;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        // Методы для авторизованных пользователей
        public async Task<UserDto> GetUserProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateUserProfileAsync(int userId, UpdateUserDto updateDto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.FullName = updateDto.FullName;
            user.Email = updateDto.Email;
            user.Address = updateDto.Address;
            user.PassportData = updateDto.PassportData;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<PaymentDto>> GetUserPaymentsAsync(int userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserAsync(userId);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<IEnumerable<TariffDto>> GetUserTariffsAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.Tariffs == null)
                return new List<TariffDto>();

            return _mapper.Map<IEnumerable<TariffDto>>(user.Tariffs);
        }

        public async Task<bool> AddBalanceAsync(int userId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма должна быть больше нуля");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            user.Balance += amount;

            // Создаем запись о платеже
            var payment = new Payment
            {
                AppUserId = userId,
                Amount = amount,
                PaymentMethod = PaymentMethod.Online,
                Status = PaymentStatus.Completed,
                PaymentDateTime = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<decimal> GetUserBalanceAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.Balance ?? 0;
        }

        // Статистика
        public async Task<Dictionary<string, int>> GetUserStatisticsAsync()
        {
            var totalUsers = await _userRepository.CountAsync();
            var activeUsers = await _userRepository.GetActiveUsersCountAsync();
            var adminUsers = (await _userRepository.FindAsync(u => u.Role == "Admin")).Count();
            var lowBalanceUsers = (await _userRepository.GetUsersWithLowBalanceAsync(100)).Count();

            return new Dictionary<string, int>
            {
                { "Всего пользователей", totalUsers },
                { "Активных пользователей", activeUsers },
                { "Администраторов", adminUsers },
                { "Пользователей с низким балансом", lowBalanceUsers },
                { "Заблокированных пользователей", totalUsers - activeUsers }
            };
        }
    }
}