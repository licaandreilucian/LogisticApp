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

    public async Task<List<AssignmentResult>> AssignDriversAndSaveAsync()
    {
     
        var orders = await _context.Orders.ToListAsync();
        var drivers = await _context.Drivers.ToListAsync();
    
        var results = new List<AssignmentResult>();

        var driverLoad = drivers.ToDictionary(d => d.Id, d => 0);
 
        foreach (var order in orders)
        {
            // Skip if already assigned
            if (await _context.AssignmentResults.AnyAsync(ar => ar.OrderId == order.Id))
                continue;

            var availableDriver = drivers
                .FirstOrDefault(d => driverLoad[d.Id] < d.MaxDeliveriesPerDay);

            if (availableDriver != null)
            {
                driverLoad[availableDriver.Id]++;
                order.DriverId = availableDriver.Id;
                var assignment = new AssignmentResult
                {
                    OrderId = order.Id,
                    DriverId = availableDriver.Id,
                    IsAssigned = true,
                    Notes = $"Assigned to driver {availableDriver.Name}"
                };

                results.Add(assignment);
                _context.Add(assignment);
            }
            else
            {
                var assignment = new AssignmentResult
                {
                    OrderId = order.Id,
                    DriverId = null,
                    IsAssigned = false,
                    Notes = "No available driver (capacity full)"
                };

                results.Add(assignment);
                _context.AssignmentResults.Add(assignment);
            }
        }

        await _context.SaveChangesAsync();
        return results;
    }

    public async Task ClearAssignmentsAsync()
    {
       
        var allAssignments = await _context.AssignmentResults.ToListAsync();

        if (allAssignments.Any())
        {
            _context.AssignmentResults.RemoveRange(allAssignments);
            await _context.SaveChangesAsync();
        }
    }

}


