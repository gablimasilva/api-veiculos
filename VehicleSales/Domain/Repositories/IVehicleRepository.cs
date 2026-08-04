using Domain.Models;

namespace Domain.Repositories;

public interface IVehicleRepository
{
    Task<Vehicle> Create(Vehicle vehicle);

    Task<bool> Update(Vehicle vehicle);

    Task<Vehicle?> Get(Guid id);

    Task<IEnumerable<Vehicle>> GetAvailable();

    Task<IEnumerable<Vehicle>> GetSold();
}