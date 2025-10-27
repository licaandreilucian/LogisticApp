using LogisticApp.BusinessLogic.Services;
using LogisticApp.Components.Pages;
using LogisticApp.Data.Repositories;
using LogisticApp.Models;
using Microsoft.EntityFrameworkCore;


public class DriverService : IDriverService
{
    private readonly IDriverRepository _repo;
    private readonly IOrderRepository _orderRepo;

    public DriverService(IDriverRepository repo, IOrderRepository orderRepo)
    {
        _repo = repo;
        _orderRepo = orderRepo;
    }

    public async Task<List<DriverDto>> GetAllDriversAsync()
    {
        var drivers = await _repo.GetAllAsync();
        var orders = await _orderRepo.GetAllAsync();


        return drivers.Select(d => new DriverDto
        {
            Id = d.Id,
            Name = d.Name,
            MaxDeliveriesPerDay = d.MaxDeliveriesPerDay,
            OrdersCount = orders.Count(o => o.DriverId == d.Id)
        }).ToList();
    }

    public async Task<DriverDto?> GetDriverByIdAsync(int id)
    {
        var d = await _repo.GetByIdAsync(id);
        var o = await _orderRepo.GetAllAsync();
        return d == null ? null : new DriverDto
        {
            Id = d.Id,
            Name = d.Name,
            MaxDeliveriesPerDay = d.MaxDeliveriesPerDay,
            OrdersCount = o.Count(o => o.DriverId == d.Id)
        };
    }

    public async Task AddDriverAsync(DriverDto dto)
    {
        var entity = new Driver
        {
            Name = dto.Name,
            MaxDeliveriesPerDay = dto.MaxDeliveriesPerDay
        };
        await _repo.AddAsync(entity);
    }

    public async Task UpdateDriverAsync(DriverDto dto)
    {
        var entity = await _repo.GetByIdAsync(dto.Id);
        if (entity == null) throw new Exception("Driver not found.");

        entity.Name = dto.Name;
        entity.MaxDeliveriesPerDay = dto.MaxDeliveriesPerDay;
        await _repo.UpdateAsync(entity);
    }

    public async Task DeleteDriverAsync(int id) => await _repo.DeleteAsync(id);
}
