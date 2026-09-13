using Microsoft.AspNetCore.Mvc;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Services;

namespace PortfolioPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HoldingsController(IHoldingService holdingService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<HoldingDto>>> GetHoldings()
    {
        var holdings = await holdingService.GetHoldingsAsync();

        return Ok(holdings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HoldingDto>> GetHolding(int id)
    {
        var holding = await holdingService.GetHoldingAsync(id);

        if (holding is null)
        {
            return NotFound();
        }

        return Ok(holding);
    }

    [HttpPost]
    public async Task<ActionResult<HoldingDto>> CreateHolding(
        CreateHoldingRequest request)
    {
        var holding = await holdingService.CreateHoldingAsync(request);

        if (holding is null)
        {
            return BadRequest("The specified account does not exist.");
        }

        return CreatedAtAction(
            nameof(GetHolding),
            new { id = holding.Id },
            holding);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHolding(
        int id,
        UpdateHoldingRequest request)
    {
        var updated = await holdingService.UpdateHoldingAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHolding(int id)
    {
        var deleted = await holdingService.DeleteHoldingAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}
