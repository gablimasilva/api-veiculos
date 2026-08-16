namespace Domain.Models;

public sealed class Sale
{
    public Guid Id { get; set; }

    public Guid VehicleId { get; set; }

    public string BuyerId { get; set; } = string.Empty;

    public decimal SalePrice { get; set; }

    public DateTime PurchasedAt { get; set; }
}