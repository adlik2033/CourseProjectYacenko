using CourseProjectYacenko.Data;
using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Application>> GetApplicationsByUserAsync(int userId)
        {
            return await _dbSet
                .Where(a => a.AppUserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByStatusAsync(ApplicationStatus status)
        {
            return await _dbSet
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByTypeAsync(ApplicationType type)
        {
            return await _dbSet
                .Where(a => a.Type == type)
                .ToListAsync();
        }

        public async Task<int> GetApplicationsCountByStatusAsync(ApplicationStatus status)
        {
            return await _dbSet
                .CountAsync(a => a.Status == status);
        }

        public async Task<bool> UpdateApplicationStatusAsync(int applicationId, ApplicationStatus status, string comment = null)
        {
            var application = await GetByIdAsync(applicationId);
            if (application == null) return false;

            application.Status = status;
            if (!string.IsNullOrEmpty(comment))
            {
                application.Comment = comment;
            }

            await UpdateAsync(application);
            await SaveChangesAsync();

            return true;
        }
    }
}