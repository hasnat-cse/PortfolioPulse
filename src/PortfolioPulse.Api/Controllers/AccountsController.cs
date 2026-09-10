using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(PortfolioPulseDbContext dbContext) : ControllerBase
{
    private readonly PortfolioPulseDbContext _dbContext = dbContext;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
    {
        var accounts = await _dbContext.Accounts.ToListAsync();

        return Ok(accounts.Select(a => a.ToDto()));
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount(CreateAccountRequest request)
    {
        var account = new Account
        {
            Name = request.Name,
            Brokerage = request.Brokerage,
            AccountType = request.AccountType,
            Currency = request.Currency
        };

        _dbContext.Accounts.Add(account);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetAccount),
            new { id = account.Id },
            account.ToDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(int id)
    {
        var account = await _dbContext.Accounts.FindAsync(id);

        if (account is null)
        {
            return NotFound();
        }

        return Ok(account.ToDto());
    }

    [HttpGet("{id}/holdings")]
    public async Task<ActionResult<IEnumerable<HoldingDto>>> GetAccountHoldings(int id)
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

        return Ok(holdings.Select(h => h.ToDto()));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(int id, UpdateAccountRequest request)
    {
        var existingAccount = await _dbContext.Accounts.FindAsync(id);

        if (existingAccount is null)
        {
            return NotFound();
        }

        existingAccount.Name = request.Name;
        existingAccount.Brokerage = request.Brokerage;
        existingAccount.AccountType = request.AccountType;
        existingAccount.Currency = request.Currency;
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