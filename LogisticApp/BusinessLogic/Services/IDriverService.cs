using LogisticApp.Models;

namespace LogisticApp.BusinessLogic.Services
{
    public interface IDriverService
    {
        Task<List<DriverDto>> GetAllDriversAsync();
        Task<DriverDto?> GetDriverByIdAsync(int id);
        Task AddDriverAsync(DriverDto dto);
        Task UpdateDriverAsync(DriverDto dto);
        Task DeleteDriverAsync(int id);
    }
}
