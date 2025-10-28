using LogisticApp.Models;

namespace LogisticApp.BusinessLogic.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderAsync(int id);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<List<OrderDto>> AssignDrivers();
        Task ClearAssignmentsAsync();
    }
}
