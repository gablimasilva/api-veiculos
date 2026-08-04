using Application.UseCases.Sale;
using Application.UseCases.Vehicle;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ApplicationModuleDependency
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddScoped<IVehicleUseCase, VehicleUseCase>();

        services.AddScoped<ISaleUseCase, SaleUseCase>();

        return services;
    }
}