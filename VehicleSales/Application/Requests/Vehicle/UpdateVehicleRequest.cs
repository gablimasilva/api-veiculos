namespace Application.Requests.Vehicle;

public sealed class UpdateVehicleRequest
{
    public string? Brand { get; set; }

    public string? Model { get; set; }

    public int? Year { get; set; }

    public string? Color { get; set; }

    public decimal? Price { get; set; }
}