using Application.Requests.Vehicle;

namespace Application.UseCases.Vehicle
{
    public interface IVehicleUseCase
    {
        Task<Domain.Models.Vehicle> Create(CreateVehicleRequest request);

        Task Update(Guid id, UpdateVehicleRequest request);

        Task<Domain.Models.Vehicle?> Get(Guid id);

        Task<IEnumerable<Domain.Models.Vehicle>> GetAvailable();

        Task<IEnumerable<Domain.Models.Vehicle>> GetSold();
    }
}
