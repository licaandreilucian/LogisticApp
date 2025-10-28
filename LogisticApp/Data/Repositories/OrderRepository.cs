using LogisticApp.Components.Pages;
using LogisticApp.Data;
using LogisticApp.Data.Repositories;
using LogisticApp.Models;
using Microsoft.EntityFrameworkCore;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _context;
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllAsync() =>
        await _context.Orders.ToListAsync();

    public async Task<Order?> GetByIdAsync(int id) =>
        await _context.Orders
                             .FirstOrDefaultAsync(o => o.Id == id);

    public async Task AddAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order != null)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<OrderDto>> AssignDriversAndSaveAsync()
    {
        var orders = await _context.Orders.ToListAsync();
        var drivers = await _context.Drivers.ToListAsync();

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


