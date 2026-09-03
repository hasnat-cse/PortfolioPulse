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
}