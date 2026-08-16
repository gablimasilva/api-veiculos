using Infrastructure.Persistence.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Data;

public sealed class AppDataContext : DbContext
{
    public AppDataContext(
        DbContextOptions<AppDataContext> options)
        : base(options)
    {
    }

    public DbSet<VehicleEntity> Vehicles => Set<VehicleEntity>();

    public DbSet<SaleEntity> Sales => Set<SaleEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDataContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}