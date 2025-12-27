using CourseProjectYacenko.Data;
using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseProjectYacenko.Repository
{
    public class TariffRepository : ITariffRepository
    {
        private readonly ApplicationDbContext _context;

        public TariffRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tariff> GetByIdAsync(int id)
        {
            return await _context.Tariffs.FindAsync(id);
        }

        public async Task<Tariff> GetByIdWithServicesAsync(int id)
        {
            return await _context.Tariffs
                .Include(t => t.ConnectedServices)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tariff>> GetAllAsync()
        {
            return await _context.Tariffs.ToListAsync();
        }

        public async Task<List<Tariff>> GetAllWithServicesAsync()
        {
            return await _context.Tariffs
                .Include(t => t.ConnectedServices)
                .ToListAsync();
        }

        public async Task<Tariff> AddAsync(Tariff tariff)
        {
            _context.Tariffs.Add(tariff);
            await _context.SaveChangesAsync();
            return tariff;
        }

        public async Task<Tariff> UpdateAsync(Tariff tariff)
        {
            _context.Tariffs.Update(tariff);
            await _context.SaveChangesAsync();
            return tariff;
        }

        public async Task DeleteAsync(int id)
        {
            var tariff = await GetByIdAsync(id);
            if (tariff != null)
            {
                _context.Tariffs.Remove(tariff);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}