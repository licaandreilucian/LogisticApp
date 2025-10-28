using LogisticApp.Models;

namespace LogisticApp.Data.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllAsync();
        Task<Order?> GetByIdAsync(int id);
        Task AddAsync(Order order);
        Task UpdateAsync(Order order);
        Task DeleteAsync(int id);
        Task<List<OrderDto>> AssignDriversAndSaveAsync();
        Task ClearAssignmentsAsync();
    }
}
