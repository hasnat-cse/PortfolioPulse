using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(PortfolioPulseDbContext dbContext) : ControllerBase
{
    private readonly PortfolioPulseDbContext _dbContext = dbContext;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
    {
        var accounts = await _dbContext.Accounts.ToListAsync();
        return Ok(accounts);
    }

    [HttpPost]
    public async Task<ActionResult<Account>> CreateAccount(Account account)
    {
        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Account>> GetAccount(int id)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        if (account is null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    [HttpGet("{id}/holdings")]
    public async Task<ActionResult<IEnumerable<Holding>>> GetAccountHoldings(int id)
    {
        var accountExists = await _dbContext.Accounts
            .AnyAsync(a => a.Id == id);

        if (!accountExists)
        {
            return NotFound();
        }

        var holdings = await _dbContext.Holdings
            .Where(h => h.AccountId == id)
            .ToListAsync();

        return Ok(holdings);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(int id, Account account)
    {
        if (id != account.Id)
        {
            return BadRequest();
        }

        var existingAccount = await _dbContext.Accounts.FindAsync(id);

        if (existingAccount is null)
        {
            return NotFound();
        }

        existingAccount.Name = account.Name;
        existingAccount.Brokerage = account.Brokerage;
        existingAccount.AccountType = account.AccountType;
        existingAccount.Currency = account.Currency;
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var existingAccount = await _dbContext.Accounts.FindAsync(id);

        if (existingAccount is null)
        {
            return NotFound();
        }

        var holdingExists = await _dbContext.Holdings.AnyAsync(h => h.AccountId == id);

        if (holdingExists)
        {
            return Conflict("Cannot delete an account that has holdings.");
        }

        _dbContext.Accounts.Remove(existingAccount);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }
}