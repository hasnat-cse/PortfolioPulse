using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Services;

public sealed class AccountService(PortfolioPulseDbContext dbContext) : IAccountService
{
    public async Task<IEnumerable<AccountDto>> GetAccountsAsync()
    {
        return await dbContext.Accounts
            .Select(AccountMappings.ToDtoExpression)
            .ToListAsync();
    }

    public async Task<AccountDto?> GetAccountAsync(int id)
    {
        return await dbContext.Accounts
            .Where(account => account.Id == id)
            .Select(AccountMappings.ToDtoExpression)
            .FirstOrDefaultAsync();
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountRequest request)
    {
        var account = new Account
        {
            Name = request.Name,
            Brokerage = request.Brokerage,
            AccountType = request.AccountType,
            Currency = request.Currency
        };

        dbContext.Accounts.Add(account);

        await dbContext.SaveChangesAsync();

        return account.ToDto();
    }

    public async Task<bool> UpdateAccountAsync(int id, UpdateAccountRequest request)
    {
        var account = await dbContext.Accounts.FindAsync(id);

        if (account is null)
        {
            return false;
        }

        account.Name = request.Name;
        account.Brokerage = request.Brokerage;
        account.AccountType = request.AccountType;
        account.Currency = request.Currency;

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<DeleteAccountResult> DeleteAccountAsync(int id)
    {
        var account = await dbContext.Accounts.FindAsync(id);

        if (account is null)
        {
            return DeleteAccountResult.NotFound;
        }

        var hasHoldings = await dbContext.Holdings
            .AnyAsync(holding => holding.AccountId == id);

        if (hasHoldings)
        {
            return DeleteAccountResult.HasHoldings;
        }

        dbContext.Accounts.Remove(account);

        await dbContext.SaveChangesAsync();

        return DeleteAccountResult.Deleted;
    }


    public async Task<IEnumerable<HoldingDto>?> GetAccountHoldingsAsync(int id)
    {
        var accountExists = await dbContext.Accounts
            .AnyAsync(account => account.Id == id);

        if (!accountExists)
        {
            return null;
        }

        return await dbContext.Holdings
            .Where(holding => holding.AccountId == id)
            .Select(HoldingMappings.ToDtoExpression)
            .ToListAsync();
    }
}