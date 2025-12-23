using CourseProjectYacenko.Interfaces;
using CourseProjectYacenko.Models;

public interface IApplicationRepository : IRepository<Application>
{
    Task<IEnumerable<Application>> GetApplicationsByStatusAsync(string status);
    Task<IEnumerable<Application>> GetApplicationsBySubscriberAsync(int subscriberId);
    Task<bool> UpdateApplicationStatusAsync(int applicationId, string status, string adminComment = null);
    Task<int> GetApplicationsCountByTypeAsync(string type);
}