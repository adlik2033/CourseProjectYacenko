using AutoMapper;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class TariffService : ITariffService
    {
        private readonly ITariffRepository _tariffRepository;
        private readonly IMapper _mapper;

        public TariffService(ITariffRepository tariffRepository, IMapper mapper)
        {
            _tariffRepository = tariffRepository;
            _mapper = mapper;
        }

        public async Task<TariffDto> GetTariffAsync(int id)
        {
            var tariff = await _tariffRepository.GetTariffWithServicesAsync(id);
            return tariff == null ? null : _mapper.Map<TariffDto>(tariff);
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

        public async Task<bool> AssignTariffToUserAsync(int tariffId, int userId)
        {
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
    }
}