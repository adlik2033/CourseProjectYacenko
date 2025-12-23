using AutoMapper;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class TariffService : ITariffService
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public TariffService(
            ITariffRepository tariffRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _tariffRepository = tariffRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<TariffDto> GetTariffAsync(int id)
        {
            var tariff = await _tariffRepository.GetTariffWithServicesAsync(id);
            if (tariff == null)
                throw new KeyNotFoundException($"Тариф с id {id} не найден");

            return _mapper.Map<TariffDto>(tariff);
        }

        public async Task<IEnumerable<TariffDto>> GetAllTariffsAsync()
        {
            var tariffs = await _tariffRepository.GetTariffsWithServicesAsync();
            return _mapper.Map<IEnumerable<TariffDto>>(tariffs);
        }

        public async Task<IEnumerable<TariffDto>> GetPopularTariffsAsync(int count)
        {
            var tariffs = await _tariffRepository.GetPopularTariffsAsync(count);
            return _mapper.Map<IEnumerable<TariffDto>>(tariffs);
        }

        public async Task<TariffDto> CreateTariffAsync(CreateTariffDto createDto)
        {
            var tariff = new Tariff
            {
                Name = createDto.Name,
                Description = createDto.Description,
                MonthlyFee = createDto.MonthlyFee,
                InternetTrafficGB = createDto.InternetTrafficGB,
                MinutesCount = createDto.MinutesCount,
                SmsCount = createDto.SmsCount
            };

            await _tariffRepository.AddAsync(tariff);
            await _tariffRepository.SaveChangesAsync();

            return _mapper.Map<TariffDto>(tariff);
        }

        public async Task UpdateTariffAsync(int id, TariffDto updateDto)
        {
            var tariff = await _tariffRepository.GetByIdAsync(id);
            if (tariff == null)
                throw new KeyNotFoundException($"Тариф с id {id} не найден");

            tariff.Name = updateDto.Name;
            tariff.Description = updateDto.Description;
            tariff.MonthlyFee = updateDto.MonthlyFee;
            tariff.InternetTrafficGB = updateDto.InternetTrafficGB;
            tariff.MinutesCount = updateDto.MinutesCount;
            tariff.SmsCount = updateDto.SmsCount;

            await _tariffRepository.UpdateAsync(tariff);
            await _tariffRepository.SaveChangesAsync();
        }

        public async Task DeleteTariffAsync(int id)
        {
            var tariff = await _tariffRepository.GetByIdAsync(id);
            if (tariff == null)
                throw new KeyNotFoundException($"Тариф с id {id} не найден");

            await _tariffRepository.DeleteAsync(tariff);
            await _tariffRepository.SaveChangesAsync();
        }

        public async Task<bool> AssignTariffToUserAsync(int tariffId, int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            return await _tariffRepository.AssignTariffToUserAsync(tariffId, userId);
        }

        public async Task<bool> RemoveTariffFromUserAsync(int tariffId, int userId)
        {
            return await _tariffRepository.RemoveTariffFromUserAsync(tariffId, userId);
        }

        public async Task<IEnumerable<TariffDto>> FindMatchingTariffsAsync(int minutes, int internetGb, int sms)
        {
            var tariffs = await _tariffRepository.FindMatchingTariffsAsync(minutes, internetGb, sms);
            return _mapper.Map<IEnumerable<TariffDto>>(tariffs);
        }

        public async Task<IEnumerable<TariffDto>> GetTariffsWithServicesAsync()
        {
            var tariffs = await _tariffRepository.GetTariffsWithServicesAsync();
            return _mapper.Map<IEnumerable<TariffDto>>(tariffs);
        }
    }
}