using CourseProjectYacenko.Data;
using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public class TariffRepository : BaseRepository<Tariff>, ITariffRepository
    {
        public TariffRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Tariff>> GetTariffsWithServicesAsync()
        {
            return await _dbSet
                .Include(t => t.ConnectedServices)
                .Include(t => t.AppUser)
                .ToListAsync();
        }

        public async Task<Tariff> GetTariffWithServicesAsync(int id)
        {
            return await _dbSet
                .Include(t => t.ConnectedServices)
                .Include(t => t.AppUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tariff>> GetPopularTariffsAsync(int count)
        {
            return await _dbSet
                .Include(t => t.ConnectedServices)
                .OrderByDescending(t => t.AppUserId != null ? 1 : 0)
                .ThenBy(t => t.MonthlyFee)
                .Take(count)
                .ToListAsync();
        }

        public async Task<bool> AssignTariffToUserAsync(int tariffId, int userId)
        {
            var tariff = await GetByIdAsync(tariffId);
            if (tariff == null) return false;

            tariff.AppUserId = userId;
            await UpdateAsync(tariff);
            await SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveTariffFromUserAsync(int tariffId, int userId)
        {
            var tariff = await GetByIdAsync(tariffId);
            if (tariff == null || tariff.AppUserId != userId) return false;

            tariff.AppUserId = null;
            await UpdateAsync(tariff);
            await SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Tariff>> FindMatchingTariffsAsync(int minutes, int internetGb, int sms)
        {
            return await _dbSet
                .Include(t => t.ConnectedServices)
                .Where(t => t.MinutesCount >= minutes &&
                           t.InternetTrafficGB >= internetGb &&
                           t.SmsCount >= sms)
                .OrderBy(t => t.MonthlyFee)
                .Take(5)
                .ToListAsync();
        }

        public override async Task<Tariff> GetByIdAsync(int id)
        {
            return await GetTariffWithServicesAsync(id);
        }
    }
}