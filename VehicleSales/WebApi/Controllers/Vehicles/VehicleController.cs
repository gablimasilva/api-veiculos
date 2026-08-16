using Application.Requests.Vehicle;
using Application.UseCases.Vehicle;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.UseCases.Vehicles
{
    [ApiController]
    [Route("v1/[controller]")]
    public sealed class VehicleController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromServices] IVehicleUseCase useCase,
            [FromBody] CreateVehicleRequest request)
        {
            var vehicle = await useCase.Create(request);

            return CreatedAtAction(
                nameof(Get),
                new { id = vehicle.Id },
                vehicle);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(
            Guid id,
            [FromServices] IVehicleUseCase useCase)
        {
            var vehicle = await useCase.Get(id);

            if (vehicle is null)
                return NotFound();

            return Ok(vehicle);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable(
            [FromServices] IVehicleUseCase useCase)
        {
            return Ok(await useCase.GetAvailable());
        }

        [HttpGet("sold")]
        public async Task<IActionResult> GetSold(
            [FromServices] IVehicleUseCase useCase)
        {
            return Ok(await useCase.GetSold());
        }

        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromServices] IVehicleUseCase useCase,
        [FromBody] UpdateVehicleRequest request)
        {
            await useCase.Update(id, request);

            return NoContent();
        }
    }
}
