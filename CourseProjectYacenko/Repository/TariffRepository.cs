using CourseProjectYacenko.Data;
using CourseProjectYacenko.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProjectYacenko.Repository
{
    public class TariffRepository : ITariffRepository
    {
        private readonly ApplicationDbContext _context;

        public TariffRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tariff?> GetByIdAsync(int id)
        {
            return await _context.Tariffs
                .Include(t => t.ConnectedServices)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Tariff>> GetAllAsync()
        {
            return await _context.Tariffs
                .Include(t => t.ConnectedServices)
                .ToListAsync();
        }

        public async Task AddAsync(Tariff tariff)
        {
            await _context.Tariffs.AddAsync(tariff);
        }

        public async Task UpdateAsync(Tariff tariff)
        {
            _context.Tariffs.Update(tariff);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var tariff = await GetByIdAsync(id);
            if (tariff != null)
            {
                _context.Tariffs.Remove(tariff);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}