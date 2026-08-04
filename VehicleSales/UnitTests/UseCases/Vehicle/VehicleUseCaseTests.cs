using Application.Exceptions;
using Application.Requests.Vehicle;
using Application.UseCases.Vehicle;
using Domain.Enums;
using Domain.Repositories;
using FluentAssertions;
using Moq;

namespace UnitTests.UseCases.Vehicle;

public class VehicleUseCaseTests
{
    private readonly Mock<IVehicleRepository> _repository;
    private readonly VehicleUseCase _useCase;

    public VehicleUseCaseTests()
    {
        _repository = new Mock<IVehicleRepository>();

        _useCase = new VehicleUseCase(
            _repository.Object);
    }

    [Fact]
    public async Task Create_Should_Create_Vehicle()
    {
        var request = new CreateVehicleRequest
        {
            Brand = "Toyota",
            Model = "Corolla",
            Year = 2024,
            Color = "Prata",
            Price = 100000
        };

        _repository
            .Setup(x => x.Create(It.IsAny<Domain.Models.Vehicle>()))
            .ReturnsAsync((Domain.Models.Vehicle v) => v);

        var result = await _useCase.Create(request);

        result.Brand.Should().Be("Toyota");
        result.Status.Should().Be(VehicleStatus.Available);

        _repository.Verify(
            x => x.Create(It.IsAny<Domain.Models.Vehicle>()),
            Times.Once);
    }

    [Fact]
    public async Task Get_Should_Return_Vehicle()
    {
        var id = Guid.NewGuid();

        _repository
            .Setup(x => x.Get(id))
            .ReturnsAsync(new Domain.Models.Vehicle
            {
                Id = id
            });

        var result = await _useCase.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
    }

    [Fact]
    public async Task Update_Should_Update_Vehicle()
    {
        var id = Guid.NewGuid();

        var vehicle = new Domain.Models.Vehicle
        {
            Id = id,
            Brand = "Toyota",
            Model = "Corolla"
        };

        _repository
            .Setup(x => x.Get(id))
            .ReturnsAsync(vehicle);

        var request = new UpdateVehicleRequest
        {
            Brand = "Honda"
        };

        await _useCase.Update(id, request);

        vehicle.Brand.Should().Be("Honda");

        _repository.Verify(
            x => x.Update(vehicle),
            Times.Once);
    }

    [Fact]
    public async Task Update_Should_Throw_NotFoundException()
    {
        var id = Guid.NewGuid();

        _repository
            .Setup(x => x.Get(id))
            .ReturnsAsync((Domain.Models.Vehicle?)null);

        var request = new UpdateVehicleRequest();

        var action = async () =>
            await _useCase.Update(id, request);

        await action
            .Should()
            .ThrowAsync<NotFoundException>();
    }
}