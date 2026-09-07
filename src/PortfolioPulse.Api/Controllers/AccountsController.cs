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
}