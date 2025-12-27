using CourseProjectYacenko.DTO;
using CourseProjectYacenko.DTO.User;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class TariffService : ITariffService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITariffRepository _tariffRepository;

        public TariffService(IUserRepository userRepository, ITariffRepository tariffRepository)
        {
            _userRepository = userRepository;
            _tariffRepository = tariffRepository;
        }

        // Получить все тарифы
        public async Task<List<TariffDto>> GetAllTariffsAsync()
        {
            var tariffs = await _tariffRepository.GetAllWithServicesAsync();
            return tariffs.Select(t => new TariffDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                MonthlyFee = t.MonthlyFee,
                InternetTrafficGB = t.InternetTrafficGB,
                MinutesCount = t.MinutesCount,
                SmsCount = t.SmsCount,
                ConnectedServices = t.ConnectedServices?.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Cost = s.Cost,
                    Description = s.Description
                }).ToList() ?? new List<ServiceDto>()
            }).ToList();
        }

        // Получить тариф по ID
        public async Task<TariffDto?> GetTariffAsync(int id)
        {
            var tariff = await _tariffRepository.GetByIdWithServicesAsync(id);
            if (tariff == null) return null;

            return new TariffDto
            {
                Id = tariff.Id,
                Name = tariff.Name,
                Description = tariff.Description,
                MonthlyFee = tariff.MonthlyFee,
                InternetTrafficGB = tariff.InternetTrafficGB,
                MinutesCount = tariff.MinutesCount,
                SmsCount = tariff.SmsCount,
                ConnectedServices = tariff.ConnectedServices?.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Cost = s.Cost,
                    Description = s.Description
                }).ToList() ?? new List<ServiceDto>()
            };
        }

        // Получить тарифы пользователя
        public async Task<List<TariffDto>> GetUserTariffsAsync(int userId)
        {
            var user = await _userRepository.GetByIdWithTariffsAsync(userId);
            return user?.Tariffs
                .Select(t => new TariffDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Description = t.Description,
                    MonthlyFee = t.MonthlyFee,
                    InternetTrafficGB = t.InternetTrafficGB,
                    MinutesCount = t.MinutesCount,
                    SmsCount = t.SmsCount,
                    ConnectedServices = t.ConnectedServices?.Select(s => new ServiceDto
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Cost = s.Cost,
                        Description = s.Description
                    }).ToList() ?? new List<ServiceDto>()
                })
                .ToList() ?? new List<TariffDto>();
        }

        // Отключить тариф у пользователя
        public async Task<bool> UnsubscribeTariffAsync(int userId, int tariffId)
        {
            var user = await _userRepository.GetByIdWithTariffsAsync(userId);
            if (user == null) return false;

            var tariff = user.Tariffs.FirstOrDefault(t => t.Id == tariffId);
            if (tariff == null) return false;

            // Удаляем связь пользователя с тарифом
            user.Tariffs.Remove(tariff);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        // Подключить тариф пользователю
        public async Task<bool> SubscribeTariffAsync(int userId, int tariffId)
        {
            var user = await _userRepository.GetByIdWithTariffsAsync(userId);
            var tariff = await _tariffRepository.GetByIdAsync(tariffId);

            if (user == null || tariff == null) return false;

            // Проверяем, не подключен ли уже этот тариф
            if (user.Tariffs.Any(t => t.Id == tariffId)) return true;

            // Подключаем тариф пользователю
            user.Tariffs.Add(tariff);
            await _userRepository.SaveChangesAsync();

            return true;
        }

        // Метод для совместимости с RemoveTariffFromUserAsync
        public async Task<bool> RemoveTariffFromUserAsync(int userId, int tariffId)
        {
            return await UnsubscribeTariffAsync(userId, tariffId);
        }

        // Метод для совместимости с AssignTariffToUserAsync
        public async Task<bool> AssignTariffToUserAsync(int userId, int tariffId)
        {
            return await SubscribeTariffAsync(userId, tariffId);
        }

        // Поиск тарифов по имени
        public async Task<List<TariffDto>> SearchTariffsAsync(string searchTerm)
        {
            var tariffs = await _tariffRepository.GetAllWithServicesAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                tariffs = tariffs.Where(t =>
                    (t.Name?.ToLower().Contains(searchTerm) ?? false) ||
                    (t.Description?.ToLower().Contains(searchTerm) ?? false))
                    .ToList();
            }

            return tariffs.Select(t => new TariffDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                MonthlyFee = t.MonthlyFee,
                InternetTrafficGB = t.InternetTrafficGB,
                MinutesCount = t.MinutesCount,
                SmsCount = t.SmsCount,
                ConnectedServices = t.ConnectedServices?.Select(s => new ServiceDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Cost = s.Cost,
                    Description = s.Description
                }).ToList() ?? new List<ServiceDto>()
            }).ToList();
        }

        // Получить доступные тарифы (не подключенные пользователем)
        public async Task<List<TariffDto>> GetAvailableTariffsAsync(int userId)
        {
            var userTariffs = await GetUserTariffsAsync(userId);
            var allTariffs = await GetAllTariffsAsync();

            // Исключаем уже подключенные тарифы
            var userTariffIds = userTariffs.Select(t => t.Id).ToHashSet();

            return allTariffs
                .Where(t => !userTariffIds.Contains(t.Id))
                .ToList();
        }

        // Проверить, подключен ли тариф у пользователя
        public async Task<bool> IsTariffSubscribedAsync(int userId, int tariffId)
        {
            var userTariffs = await GetUserTariffsAsync(userId);
            return userTariffs.Any(t => t.Id == tariffId);
        }
    }
}