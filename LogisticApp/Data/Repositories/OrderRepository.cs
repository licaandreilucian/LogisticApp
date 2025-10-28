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
}



