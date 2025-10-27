using LogisticApp.Models;

namespace LogisticApp.Data.Repositories
{
    public interface IDriverRepository
    {
      
            Task<List<Driver>> GetAllAsync();
            Task<Driver?> GetByIdAsync(int id);
            Task AddAsync(Driver driver);
            Task UpdateAsync(Driver driver);
            Task DeleteAsync(int id);
        
    }
}
