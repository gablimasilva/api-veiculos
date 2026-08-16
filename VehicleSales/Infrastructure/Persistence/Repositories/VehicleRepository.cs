using Domain.Enums;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public sealed class VehicleRepository : IVehicleRepository
{
    private readonly AppDataContext _context;

    public VehicleRepository(AppDataContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> Create(Vehicle vehicle)
    {
        var entity = new VehicleEntity
        {
            Id = vehicle.Id,
            Brand = vehicle.Brand,
            Model = vehicle.Model,
            Year = vehicle.Year,
            Color = vehicle.Color,
            Price = vehicle.Price,
            Status = (int)vehicle.Status,
            CreatedAt = vehicle.CreatedAt
        };

        _context.Vehicles.Add(entity);

        await _context.SaveChangesAsync();

        return vehicle;
    }

    public async Task<Vehicle?> Get(Guid id)
    {
        var entity = await _context.Vehicles
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
            return null;

        return Map(entity);
    }

    public async Task<IEnumerable<Vehicle>> GetAvailable()
    {
        return await _context.Vehicles
            .Where(x => x.Status == (int)VehicleStatus.Available)
            .OrderBy(x => x.Price)
            .Select(x => Map(x))
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetSold()
    {
        return await _context.Vehicles
            .Where(x => x.Status == (int)VehicleStatus.Sold)
            .OrderBy(x => x.Price)
            .Select(x => Map(x))
            .ToListAsync();
    }

    public async Task<bool> Update(Vehicle vehicle)
    {
        var entity = await _context.Vehicles
            .FirstOrDefaultAsync(x => x.Id == vehicle.Id);

        if (entity == null)
            return false;

        entity.Brand = vehicle.Brand;
        entity.Model = vehicle.Model;
        entity.Year = vehicle.Year;
        entity.Color = vehicle.Color;
        entity.Price = vehicle.Price;
        entity.Status = (int)vehicle.Status;

        await _context.SaveChangesAsync();

        return true;
    }

    private static Vehicle Map(VehicleEntity entity)
    {
        return new Vehicle
        {
            Id = entity.Id,
            Brand = entity.Brand,
            Model = entity.Model,
            Year = entity.Year,
            Color = entity.Color,
            Price = entity.Price,
            Status = (VehicleStatus)entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }
}