using Application.Exceptions;
using Application.Requests.Sale;
using Application.UseCases.Sale;
using Domain.Enums;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace UnitTests.UseCases.Sale;

public class SaleUseCaseTests
{
    private readonly Mock<ISaleRepository> _saleRepository;

    private readonly Mock<IVehicleRepository> _vehicleRepository;

    private readonly SaleUseCase _useCase;

    public SaleUseCaseTests()
    {
        _saleRepository = new Mock<ISaleRepository>();

        _vehicleRepository = new Mock<IVehicleRepository>();

        _useCase = new SaleUseCase(
            _saleRepository.Object,
            _vehicleRepository.Object);
    }

    [Fact]
    public async Task Purchase_Should_Create_Sale()
    {
        var vehicleId = Guid.NewGuid();

        _vehicleRepository
            .Setup(x => x.Get(vehicleId))
            .ReturnsAsync(new Domain.Models.Vehicle
            {
                Id = vehicleId,
                Price = 50000,
                Status = VehicleStatus.Available
            });

        var request = new PurchaseVehicleRequest
        {
            VehicleId = vehicleId
        };

        var sale = await _useCase.Purchase(
            "LOCAL-TEST",
            request);

        sale.VehicleId.Should().Be(vehicleId);

        _saleRepository.Verify(
            x => x.Create(It.IsAny<Domain.Models.Sale>()),
            Times.Once);

        _vehicleRepository.Verify(
            x => x.Update(It.IsAny<Domain.Models.Vehicle>()),
            Times.Once);
    }

    [Fact]
    public async Task Purchase_Should_Throw_NotFoundException()
    {
        var vehicleId = Guid.NewGuid();

        _vehicleRepository
            .Setup(x => x.Get(vehicleId))
            .ReturnsAsync((Domain.Models.Vehicle?)null);

        var request = new PurchaseVehicleRequest
        {
            VehicleId = vehicleId
        };

        var action = async () =>
            await _useCase.Purchase(
                "LOCAL-TEST",
                request);

        await action
            .Should()
            .ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Purchase_Should_Throw_BusinessException()
    {
        var vehicleId = Guid.NewGuid();

        _vehicleRepository
            .Setup(x => x.Get(vehicleId))
            .ReturnsAsync(new Domain.Models.Vehicle
            {
                Id = vehicleId,
                Status = VehicleStatus.Sold
            });

        var request = new PurchaseVehicleRequest
        {
            VehicleId = vehicleId
        };

        var action = async () =>
            await _useCase.Purchase(
                "LOCAL-TEST",
                request);

        await action
            .Should()
            .ThrowAsync<BusinessException>();
    }
}