namespace Infrastructure.Persistence.Entity;

public sealed class SaleEntity
{
    public Guid Id { get; set; }

    public Guid VehicleId { get; set; }

    public string BuyerId { get; set; } = string.Empty;

    public decimal SalePrice { get; set; }

    public DateTime PurchasedAt { get; set; }

    public VehicleEntity Vehicle { get; set; } = null!;
}