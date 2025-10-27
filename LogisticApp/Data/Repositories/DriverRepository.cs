using LogisticApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticApp.Data.Repositories
{

    public class DriverRepository : IDriverRepository
    {
        private readonly AppDbContext _context;
        public DriverRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Driver>> GetAllAsync() =>
            await _context.Drivers.Include(d => d.Orders)
        .ToListAsync();

        public async Task<Driver?> GetByIdAsync(int id) =>
            await _context.Drivers.FindAsync(id);

        public async Task AddAsync(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var driver = await _context.Drivers.FindAsync(id);
            if (driver != null)
            {
                _context.Drivers.Remove(driver);
                await _context.SaveChangesAsync();
            }
        }
    }
}

