using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Models;
using PortfolioPulse.Api.Services;

namespace PortfolioPulse.Api.Tests.Services;

public class AccountServiceTests
{
    private static PortfolioPulseDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<PortfolioPulseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PortfolioPulseDbContext(options);
    }

    [Fact]
    public async Task GetAccountsAsync_ReturnsAccounts()
    {
        await using var dbContext = CreateDbContext();

        dbContext.Accounts.Add(new()
        {
            Name = "Test Account",
            Brokerage = "Test Brokerage",
            AccountType = "TFSA",
            Currency = "CAD"
        });

        await dbContext.SaveChangesAsync();

        var service = new AccountService(dbContext);

        var result = await service.GetAccountsAsync();

        Assert.Single(result);

        var account = result.Single();

        Assert.Equal("Test Account", account.Name);
        Assert.Equal("Test Brokerage", account.Brokerage);
        Assert.Equal("TFSA", account.AccountType);
        Assert.Equal("CAD", account.Currency);
    }

    [Fact]
    public async Task CreateAccountAsync_CreatesAndReturnsAccount()
    {
        await using var dbContext = CreateDbContext();

        var service = new AccountService(dbContext);

        var request = new CreateAccountRequest(
            "New Account",
            "Test Brokerage",
            "TFSA");

        var result = await service.CreateAccountAsync(request);

        Assert.NotEqual(0, result.Id);
        Assert.Equal("New Account", result.Name);
        Assert.Equal("Test Brokerage", result.Brokerage);
        Assert.Equal("TFSA", result.AccountType);
        Assert.Equal("CAD", result.Currency);

        var savedAccount = await dbContext.Accounts
            .SingleAsync();

        Assert.Equal(result.Id, savedAccount.Id);
        Assert.Equal("New Account", savedAccount.Name);
    }

    [Fact]
    public async Task GetAccountAsync_WhenAccountExists_ReturnsAccount()
    {
        await using var dbContext = CreateDbContext();

        var account = new Account
        {
            Name = "Test Account",
            Brokerage = "Test Brokerage",
            AccountType = "TFSA",
            Currency = "CAD"
        };

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();

        var service = new AccountService(dbContext);

        var result = await service.GetAccountAsync(account.Id);

        Assert.NotNull(result);
        Assert.Equal(account.Id, result.Id);
        Assert.Equal("Test Account", result.Name);
    }

    [Fact]
    public async Task GetAccountAsync_WhenAccountDoesNotExist_ReturnsNull()
    {
        await using var dbContext = CreateDbContext();

        var service = new AccountService(dbContext);

        var result = await service.GetAccountAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAccountAsync_WhenAccountExists_UpdatesAccount()
    {
        await using var dbContext = CreateDbContext();

        var account = new Account
        {
            Name = "Original Account",
            Brokerage = "Original Brokerage",
            AccountType = "TFSA",
            Currency = "CAD"
        };

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();

        var service = new AccountService(dbContext);

        var request = new UpdateAccountRequest(
            "Updated Account",
            "Updated Brokerage",
            "RRSP",
            "USD");

        var result = await service.UpdateAccountAsync(account.Id, request);

        Assert.True(result);

        var updatedAccount = await dbContext.Accounts
            .SingleAsync(a => a.Id == account.Id);

        Assert.Equal("Updated Account", updatedAccount.Name);
        Assert.Equal("Updated Brokerage", updatedAccount.Brokerage);
        Assert.Equal("RRSP", updatedAccount.AccountType);
        Assert.Equal("USD", updatedAccount.Currency);
    }

    [Fact]
    public async Task UpdateAccountAsync_WhenAccountDoesNotExist_ReturnsFalse()
    {
        await using var dbContext = CreateDbContext();

        var service = new AccountService(dbContext);

        var request = new UpdateAccountRequest(
            "Updated Account",
            "Updated Brokerage",
            "RRSP",
            "CAD");

        var result = await service.UpdateAccountAsync(999, request);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAccountAsync_WhenAccountExistsWithoutHoldings_DeletesAccount()
    {
        await using var dbContext = CreateDbContext();

        var account = new Account
        {
            Name = "Test Account",
            Brokerage = "Test Brokerage",
            AccountType = "TFSA",
            Currency = "CAD"
        };

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();

        var service = new AccountService(dbContext);

        var result = await service.DeleteAccountAsync(account.Id);

        Assert.Equal(DeleteAccountResult.Deleted, result);

        var deletedAccount = await dbContext.Accounts
            .FindAsync(account.Id);

        Assert.Null(deletedAccount);
    }

    [Fact]
    public async Task DeleteAccountAsync_WhenAccountDoesNotExist_ReturnsNotFound()
    {
        await using var dbContext = CreateDbContext();

        var service = new AccountService(dbContext);

        var result = await service.DeleteAccountAsync(999);

        Assert.Equal(DeleteAccountResult.NotFound, result);
    }

    [Fact]
    public async Task DeleteAccountAsync_WhenAccountHasHoldings_ReturnsHasHoldings()
    {
        await using var dbContext = CreateDbContext();

        var account = new Account
        {
            Name = "Test Account",
            Brokerage = "Test Brokerage",
            AccountType = "TFSA",
            Currency = "CAD"
        };

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();

        dbContext.Holdings.Add(new Holding
        {
            AccountId = account.Id,
            Symbol = "HLAL",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        });

        await dbContext.SaveChangesAsync();

        var service = new AccountService(dbContext);

        var result = await service.DeleteAccountAsync(account.Id);

        Assert.Equal(DeleteAccountResult.HasHoldings, result);

        var existingAccount = await dbContext.Accounts
            .FindAsync(account.Id);

        Assert.NotNull(existingAccount);

        var existingHolding = await dbContext.Holdings
            .SingleAsync(h => h.AccountId == account.Id);

        Assert.Equal("HLAL", existingHolding.Symbol);
    }

    [Fact]
    public async Task GetAccountHoldingsAsync_WhenAccountExists_ReturnsItsHoldings()
    {
        await using var dbContext = CreateDbContext();

        var account = new Account
        {
            Name = "Test Account",
            Brokerage = "Test Brokerage",
            AccountType = "TFSA",
            Currency = "CAD"
        };

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();

        dbContext.Holdings.AddRange(
            new Holding
            {
                AccountId = account.Id,
                Symbol = "HLAL",
                Quantity = 25,
                AverageCost = 95.42m,
                Currency = "USD"
            },
            new Holding
            {
                AccountId = account.Id,
                Symbol = "SPUS",
                Quantity = 10,
                AverageCost = 75.00m,
                Currency = "USD"
            });

        await dbContext.SaveChangesAsync();

        var service = new AccountService(dbContext);

        var result = await service.GetAccountHoldingsAsync(account.Id);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());

        Assert.Contains(result, holding => holding.Symbol == "HLAL");
        Assert.Contains(result, holding => holding.Symbol == "SPUS");
    }

    [Fact]
    public async Task GetAccountHoldingsAsync_WhenAccountDoesNotExist_ReturnsNull()
    {
        await using var dbContext = CreateDbContext();

        var service = new AccountService(dbContext);

        var result = await service.GetAccountHoldingsAsync(999);

        Assert.Null(result);
    }
}
