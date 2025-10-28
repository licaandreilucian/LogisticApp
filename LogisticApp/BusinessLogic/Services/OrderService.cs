using LogisticApp.BusinessLogic.Services;
using LogisticApp.Data;
using LogisticApp.Data.Repositories;
using LogisticApp.Models;
using Microsoft.EntityFrameworkCore;


public class OrderService : IOrderService
{
    private readonly IOrderRepository _repo;
    private readonly AppDbContext _context;
    public OrderService(IOrderRepository repo, AppDbContext context)
    {
        _repo = repo;     
        _context = context;
    }

    public Task<List<Order>> GetAllOrdersAsync() => _repo.GetAllAsync();
    public Task<Order?> GetOrderAsync(int id) => _repo.GetByIdAsync(id);
    public Task AddOrderAsync(Order order) => _repo.AddAsync(order);
    public Task UpdateOrderAsync(Order order) => _repo.UpdateAsync(order);
    public Task DeleteOrderAsync(int id) => _repo.DeleteAsync(id);
    public async Task<List<OrderDto>> AssignDrivers()
    {
        var orders = await _context.Orders.ToListAsync();
        var drivers =  await _context.Drivers.ToListAsync();

        var results = new List<OrderDto>();
        var driverLoad = drivers.ToDictionary(d => d.Id, d => 0);

        foreach (var order in orders)
        {
            var availableDriver = drivers
                .FirstOrDefault(d => driverLoad[d.Id] < d.MaxDeliveriesPerDay);

            if (availableDriver != null)
            {
                driverLoad[availableDriver.Id]++;
                order.DriverId = availableDriver.Id;

                var dto = new OrderDto
                {
                    Id = order.Id,
                    CustomerName = order.CustomerName,
                    DestinationCity = order.DestinationCity,
                    Weight = order.Weight,
                    DriverId = availableDriver.Id,
                    DriverName = availableDriver.Name,
                    MaxDeliveriesPerDay = availableDriver.MaxDeliveriesPerDay,
                    Notes = $"Assigned to driver {availableDriver.Name}"
                };

                results.Add(dto);
            }
            else
            {
                var dto = new OrderDto
                {
                    Id = order.Id,
                    CustomerName = order.CustomerName,
                    DestinationCity = order.DestinationCity,
                    Weight = order.Weight,
                    DriverId = null,
                    DriverName = "Unassigned",
                    Notes = "No available driver (capacity full)"
                };

                results.Add(dto);

            }
            _context.Orders.Update(order);
        }

        await _context.SaveChangesAsync();
        return results;

    }
    public async Task ClearAssignmentsAsync()
    {
        // Get all orders that have a driver assigned
        var assignedOrders = await _context.Orders
            .Where(o => o.DriverId != null)
            .ToListAsync();

        if (assignedOrders.Any())
        {
            foreach (var order in assignedOrders)
            {
                order.DriverId = null;  // Unassign driver
            }

            await _context.SaveChangesAsync();
        }
    } 
}

    


