using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HoldingsController(PortfolioPulseDbContext dbContext) : ControllerBase
{
    private readonly PortfolioPulseDbContext _dbContext = dbContext;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<HoldingDto>>> GetHoldings()
    {
        var holdings = await _dbContext.Holdings
            .Select(HoldingMappings.ToDtoExpression)
            .ToListAsync();

        return Ok(holdings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<HoldingDto>> GetHolding(int id)
    {
        var holding = await _dbContext.Holdings
            .Where(h => h.Id == id)
            .Select(HoldingMappings.ToDtoExpression)
            .FirstOrDefaultAsync();

        if (holding is null)
        {
            return NotFound();
        }

        return Ok(holding);
    }

    [HttpPost]
    public async Task<ActionResult<HoldingDto>> CreateHolding(CreateHoldingRequest request)
    {
        var holding = new Holding
        {
            AccountId = request.AccountId,
            Symbol = request.Symbol,
            Quantity = request.Quantity,
            AverageCost = request.AverageCost,
            Currency = request.Currency
        };

        var accountExists = await _dbContext.Accounts.AnyAsync(a => a.Id == holding.AccountId);

        if (!accountExists)
        {
            return BadRequest("The specified account does not exist.");
        }

        _dbContext.Holdings.Add(holding);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetHolding),
            new { id = holding.Id },
            holding.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHolding(int id, UpdateHoldingRequest request)
    {
        var existingHolding = await _dbContext.Holdings.FindAsync(id);

        if (existingHolding is null)
        {
            return NotFound();
        }

        existingHolding.Symbol = request.Symbol;
        existingHolding.Quantity = request.Quantity;
        existingHolding.AverageCost = request.AverageCost;
        existingHolding.Currency = request.Currency;
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHolding(int id)
    {
        var holding = await _dbContext.Holdings.FindAsync(id);

        if (holding is null)
        {
            return NotFound();
        }

        _dbContext.Holdings.Remove(holding);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}