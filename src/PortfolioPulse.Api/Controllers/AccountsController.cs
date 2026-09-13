using Microsoft.AspNetCore.Mvc;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Services;

namespace PortfolioPulse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController(IAccountService accountService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts()
    {
        var accounts = await accountService.GetAccountsAsync();

        return Ok(accounts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountDto>> GetAccount(int id)
    {
        var account = await accountService.GetAccountAsync(id);

        if (account is null)
        {
            return NotFound();
        }

        return Ok(account);
    }

    [HttpPost]
    public async Task<ActionResult<AccountDto>> CreateAccount(
        CreateAccountRequest request)
    {
        var account = await accountService.CreateAccountAsync(request);

        return CreatedAtAction(
            nameof(GetAccount),
            new { id = account.Id },
            account);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(
        int id,
        UpdateAccountRequest request)
    {
        var updated = await accountService.UpdateAccountAsync(id, request);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var result = await accountService.DeleteAccountAsync(id);

        return result switch
        {
            DeleteAccountResult.NotFound => NotFound(),

            DeleteAccountResult.HasHoldings =>
                Conflict("Cannot delete an account that has holdings."),

            DeleteAccountResult.Deleted => NoContent(),

            _ => Problem()
        };
    }

    [HttpGet("{id}/holdings")]
    public async Task<ActionResult<IEnumerable<HoldingDto>>> GetAccountHoldings(
        int id)
    {
        var holdings = await accountService.GetAccountHoldingsAsync(id);

        if (holdings is null)
        {
            return NotFound();
        }

        return Ok(holdings);
    }
}