using Application.Requests.Vehicle;
using Domain.Repositories;
using Domain.Enums;

namespace Application.UseCases.Vehicle
{
    public sealed class VehicleUseCase : IVehicleUseCase
    {
        private readonly IVehicleRepository _repository;

        public VehicleUseCase(
            IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Domain.Models.Vehicle> Create(
            CreateVehicleRequest request)
        {
            var vehicle = new Domain.Models.Vehicle
            {
                Id = Guid.NewGuid(),
                Brand = request.Brand,
                Model = request.Model,
                Year = request.Year,
                Color = request.Color,
                Price = request.Price,
                Status = VehicleStatus.Available,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.Create(vehicle);
        }

        public async Task<Domain.Models.Vehicle?> Get(Guid id)
        {
            return await _repository.Get(id);
        }

        public async Task<IEnumerable<Domain.Models.Vehicle>> GetAvailable()
        {
            return await _repository.GetAvailable();
        }

        public async Task<IEnumerable<Domain.Models.Vehicle>> GetSold()
        {
            return await _repository.GetSold();
        }

        public async Task Update(
            Guid id,
            UpdateVehicleRequest request)
        {
            var vehicle = await _repository.Get(id);

            if (vehicle is null)
                throw new KeyNotFoundException(
                    "Vehicle not found.");

            if (!string.IsNullOrWhiteSpace(request.Brand))
                vehicle.Brand = request.Brand;

            if (!string.IsNullOrWhiteSpace(request.Model))
                vehicle.Model = request.Model;

            if (request.Year.HasValue)
                vehicle.Year = request.Year.Value;

            if (!string.IsNullOrWhiteSpace(request.Color))
                vehicle.Color = request.Color;

            if (request.Price.HasValue)
                vehicle.Price = request.Price.Value;

            await _repository.Update(vehicle);
        }
    }
}
