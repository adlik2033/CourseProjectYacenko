using CourseProjectYacenko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Task<IEnumerable<Application>> GetApplicationsByUserAsync(int userId);
        Task<IEnumerable<Application>> GetApplicationsByStatusAsync(ApplicationStatus status);
        Task<IEnumerable<Application>> GetApplicationsByTypeAsync(ApplicationType type);
        Task<int> GetApplicationsCountByStatusAsync(ApplicationStatus status);
        Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, string comment = null);
    }
}