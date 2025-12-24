using AutoMapper;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<PaymentDto> GetPaymentAsync(int id)
        {
            var payment = await _paymentRepository.GetByIdAsync(id);
            return payment == null ? null : _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByUserAsync(int userId)
        {
            var payments = await _paymentRepository.GetPaymentsByUserAsync(userId);
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<PaymentDto> CreatePaymentAsync(int userId, decimal amount, string paymentMethod)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var payment = new Payment
            {
                AppUserId = userId,
                Amount = amount,
                PaymentMethod = Enum.Parse<PaymentMethod>(paymentMethod),
                Status = PaymentStatus.Completed,
                PaymentDateTime = DateTime.UtcNow
            };

            // Обновляем баланс пользователя
            user.Balance += amount;

            await _paymentRepository.AddAsync(payment);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<PaymentDto>(payment);
        }

        public async Task<IEnumerable<PaymentDto>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PaymentDto>>(payments);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _paymentRepository.GetTotalRevenueAsync();
        }

        public async Task<decimal> GetUserTotalPaidAsync(int userId)
        {
            return await _paymentRepository.GetUserTotalPaidAsync(userId);
        }
    }
}