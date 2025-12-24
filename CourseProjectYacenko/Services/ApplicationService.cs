using AutoMapper;
using CourseProjectYacenko.DTO;
using CourseProjectYacenko.Models;
using CourseProjectYacenko.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ApplicationService(
            IApplicationRepository applicationRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _applicationRepository = applicationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ApplicationDto> GetApplicationAsync(int id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);
            return application == null ? null : _mapper.Map<ApplicationDto>(application);
        }

        public async Task<IEnumerable<ApplicationDto>> GetApplicationsByUserAsync(int userId)
        {
            var applications = await _applicationRepository.GetApplicationsByUserAsync(userId);
            return _mapper.Map<IEnumerable<ApplicationDto>>(applications);
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync()
        {
            var applications = await _applicationRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ApplicationDto>>(applications);
        }

        public async Task<ApplicationDto> CreateApplicationAsync(int userId, ApplicationType type, string comment)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var application = new Application
            {
                AppUserId = userId,
                Type = type,
                Comment = comment,
                Status = ApplicationStatus.New,
                CreationDate = System.DateTime.Now
            };

            await _applicationRepository.AddAsync(application);
            await _applicationRepository.SaveChangesAsync();

            return _mapper.Map<ApplicationDto>(application);
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, string comment = null)
        {
            return await _applicationRepository.UpdateApplicationStatusAsync(applicationId, status, comment);
        }
    }
}