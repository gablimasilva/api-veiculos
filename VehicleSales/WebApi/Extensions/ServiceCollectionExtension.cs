using Infrastructure;

namespace WebApi.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddProjectDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        return services;
    }
}