using LogisticApp.BusinessLogic.Services;
using LogisticApp.Data;
using LogisticApp.Data.Repositories;
using LogisticApp.Models;
using Microsoft.EntityFrameworkCore;


public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo)
    {
        _repo = repo;
    }

    public Task<List<Order>> GetAllOrdersAsync() => _repo.GetAllAsync();
    public Task<Order?> GetOrderAsync(int id) => _repo.GetByIdAsync(id);
    public Task AddOrderAsync(Order order) => _repo.AddAsync(order);
    public Task UpdateOrderAsync(Order order) => _repo.UpdateAsync(order);
    public Task DeleteOrderAsync(int id) => _repo.DeleteAsync(id);
    public Task<List<AssignmentResult>> AssignDrivers() => _repo.AssignDriversAndSaveAsync();
    public Task ClearAssignmentsAsync() => _repo.ClearAssignmentsAsync();

}

    


