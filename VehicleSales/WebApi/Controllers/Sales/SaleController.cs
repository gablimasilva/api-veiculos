using Application.UseCases.Sale;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Requests.Sale;
using Application.Exceptions;

namespace WebApi.Controllers;

[ApiController]
[Route("v1/[controller]")]
public sealed class SaleController : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Purchase(
        [FromServices] ISaleUseCase useCase,
        [FromBody] PurchaseVehicleRequest request)
    {
        var buyerId =
            User.FindFirst("sub")?.Value
            ?? throw new UnauthorizedException("User not authenticated.");

        var sale = await useCase.Purchase(
            buyerId,
            request);

        return Ok(sale);
    }
}