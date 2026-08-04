namespace Infrastructure.Persistence.Entity;

public sealed class VehicleEntity
{
    public Guid Id { get; set; }

    public string Brand { get; set; } = string.Empty;

    public string Model { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Color { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }
}