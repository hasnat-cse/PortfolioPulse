using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HoldingsController(PortfolioPulseDbContext dbContext) : ControllerBase
{
    private readonly PortfolioPulseDbContext _dbContext = dbContext;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Holding>>> GetHoldings()
    {
        var holdings = await _dbContext.Holdings.ToListAsync();
        return Ok(holdings);
    }

    [HttpPost]
    public async Task<ActionResult<Holding>> CreateHolding(Holding holding)
    {
        var accountExists = await _dbContext.Accounts.AnyAsync(a => a.Id == holding.AccountId);

        if (!accountExists)
        {
            return BadRequest("The specified account does not exist.");
        }

        _dbContext.Holdings.Add(holding);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHolding), new { id = holding.Id }, holding);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Holding>> GetHolding(int id)
    {
        var holding = await _dbContext.Holdings.FindAsync(id);

        if (holding is null)
        {
            return NotFound();
        }

        return Ok(holding);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHolding(int id, Holding holding)
    {
        if (id != holding.Id)
        {
            return BadRequest();
        }

        var existingHolding = await _dbContext.Holdings.FindAsync(id);

        if (existingHolding is null)
        {
            return NotFound();
        }

        existingHolding.Symbol = holding.Symbol;
        existingHolding.Quantity = holding.Quantity;
        existingHolding.AverageCost = holding.AverageCost;
        existingHolding.Currency = holding.Currency;
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