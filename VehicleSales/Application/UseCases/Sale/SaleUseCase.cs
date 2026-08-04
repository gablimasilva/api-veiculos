using Application.Exceptions;
using Application.Requests.Sale;
using Domain.Enums;
using Domain.Repositories;

namespace Application.UseCases.Sale;

public sealed class SaleUseCase : ISaleUseCase
{
    private readonly ISaleRepository _saleRepository;
    private readonly IVehicleRepository _vehicleRepository;

    public SaleUseCase(
        ISaleRepository saleRepository,
        IVehicleRepository vehicleRepository)
    {
        _saleRepository = saleRepository;
        _vehicleRepository = vehicleRepository;
    }

    public async Task<Domain.Models.Sale> Purchase(
        string buyerId,
        PurchaseVehicleRequest request)
    {
        var vehicle = await _vehicleRepository.Get(request.VehicleId);

        if (vehicle is null)
            throw new NotFoundException("Vehicle not found.");

        if (vehicle.Status == VehicleStatus.Sold)
            throw new BusinessException("Vehicle already sold.");

        var sale = new Domain.Models.Sale
        {
            Id = Guid.NewGuid(),
            VehicleId = vehicle.Id,
            BuyerId = buyerId,
            SalePrice = vehicle.Price,
            PurchasedAt = DateTime.UtcNow
        };

        await _saleRepository.Create(sale);

        vehicle.Status = VehicleStatus.Sold;

        await _vehicleRepository.Update(vehicle);

        return sale;
    }
}