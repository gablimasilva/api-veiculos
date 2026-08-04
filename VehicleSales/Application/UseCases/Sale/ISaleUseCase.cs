using Application.Requests.Sale;
using Domain.Models;

namespace Application.UseCases.Sale;

public interface ISaleUseCase
{
    Task<Domain.Models.Sale> Purchase(
        string buyerId,
        PurchaseVehicleRequest request);
}