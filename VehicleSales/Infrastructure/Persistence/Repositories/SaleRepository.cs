using Domain.Models;
using Domain.Repositories;
using Infrastructure.Persistence.Data;
using Infrastructure.Persistence.Entity;

namespace Infrastructure.Persistence.Repositories;

public sealed class SaleRepository : ISaleRepository
{
    private readonly AppDataContext _context;

    public SaleRepository(AppDataContext context)
    {
        _context = context;
    }

    public async Task<Sale> Create(Sale sale)
    {
        var entity = new SaleEntity
        {
            Id = sale.Id,
            VehicleId = sale.VehicleId,
            BuyerId = sale.BuyerId,
            SalePrice = sale.SalePrice,
            PurchasedAt = sale.PurchasedAt
        };

        _context.Sales.Add(entity);

        await _context.SaveChangesAsync();

        return sale;
    }
}