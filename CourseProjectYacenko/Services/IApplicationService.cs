using CourseProjectYacenko.DTO;
using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Services
{
    public interface IApplicationService
    {
        Task<ApplicationDto> GetApplicationAsync(int id);
        Task<IEnumerable<ApplicationDto>> GetApplicationsByUserAsync(int userId);
        Task<IEnumerable<ApplicationDto>> GetAllApplicationsAsync();
        Task<ApplicationDto> CreateApplicationAsync(int userId, ApplicationType type, string comment);
        Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, string comment = null);
    }
}