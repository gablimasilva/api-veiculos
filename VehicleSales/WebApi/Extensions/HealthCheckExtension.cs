namespace WebApi.Extensions
{
    public static class HealthCheckExtension
    {
        public static IServiceCollection AddApiHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddNpgSql(
                    configuration.GetConnectionString("DefaultConnection")!);

            return services;
        }
    }
}
