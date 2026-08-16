using Application.UseCases.Sale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Requests.Sale;

namespace WebApi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public sealed class SaleController : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Purchase(
    [FromServices] ISaleUseCase useCase,
    [FromBody] PurchaseVehicleRequest request)
    {
        var sale = await useCase.Purchase(request);

        return Ok(sale);
    }
}