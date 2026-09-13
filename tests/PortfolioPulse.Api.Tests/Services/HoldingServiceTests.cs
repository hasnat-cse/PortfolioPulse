using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Models;
using PortfolioPulse.Api.Services;

namespace PortfolioPulse.Api.Tests.Services;

public class HoldingServiceTests
{
    private static PortfolioPulseDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<PortfolioPulseDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PortfolioPulseDbContext(options);
    }

    [Fact]
    public async Task GetHoldingsAsync_ReturnsHoldings()
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

        var service = new HoldingService(dbContext);

        var result = await service.GetHoldingsAsync();

        Assert.Single(result);

        var holding = result.Single();

        Assert.Equal(account.Id, holding.AccountId);
        Assert.Equal("HLAL", holding.Symbol);
        Assert.Equal(25, holding.Quantity);
        Assert.Equal(95.42m, holding.AverageCost);
        Assert.Equal("USD", holding.Currency);
    }

    [Fact]
    public async Task GetHoldingAsync_WhenHoldingExists_ReturnsHolding()
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

        var holding = new Holding
        {
            AccountId = account.Id,
            Symbol = "HLAL",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        };

        dbContext.Holdings.Add(holding);
        await dbContext.SaveChangesAsync();

        var service = new HoldingService(dbContext);

        var result = await service.GetHoldingAsync(holding.Id);

        Assert.NotNull(result);
        Assert.Equal(holding.Id, result.Id);
        Assert.Equal(account.Id, result.AccountId);
        Assert.Equal("HLAL", result.Symbol);
        Assert.Equal(25, result.Quantity);
        Assert.Equal(95.42m, result.AverageCost);
        Assert.Equal("USD", result.Currency);
    }

    [Fact]
    public async Task GetHoldingAsync_WhenHoldingDoesNotExist_ReturnsNull()
    {
        await using var dbContext = CreateDbContext();

        var service = new HoldingService(dbContext);

        var result = await service.GetHoldingAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateHoldingAsync_WhenAccountExists_CreatesAndReturnsHolding()
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

        var service = new HoldingService(dbContext);

        var request = new CreateHoldingRequest(
            account.Id,
            "HLAL",
            25,
            95.42m,
            "USD");

        var result = await service.CreateHoldingAsync(request);

        Assert.NotNull(result);
        Assert.NotEqual(0, result.Id);
        Assert.Equal(account.Id, result.AccountId);
        Assert.Equal("HLAL", result.Symbol);
        Assert.Equal(25, result.Quantity);
        Assert.Equal(95.42m, result.AverageCost);
        Assert.Equal("USD", result.Currency);

        var savedHolding = await dbContext.Holdings
            .SingleAsync();

        Assert.Equal(result.Id, savedHolding.Id);
        Assert.Equal(account.Id, savedHolding.AccountId);
        Assert.Equal("HLAL", savedHolding.Symbol);
    }

    [Fact]
    public async Task CreateHoldingAsync_WhenAccountDoesNotExist_ReturnsNull()
    {
        await using var dbContext = CreateDbContext();

        var service = new HoldingService(dbContext);

        var request = new CreateHoldingRequest(
            999,
            "HLAL",
            25,
            95.42m,
            "USD");

        var result = await service.CreateHoldingAsync(request);

        Assert.Null(result);

        Assert.Empty(dbContext.Holdings);
    }

    [Fact]
    public async Task UpdateHoldingAsync_WhenHoldingExists_UpdatesHolding()
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

        var holding = new Holding
        {
            AccountId = account.Id,
            Symbol = "HLAL",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        };

        dbContext.Holdings.Add(holding);
        await dbContext.SaveChangesAsync();

        var service = new HoldingService(dbContext);

        var request = new UpdateHoldingRequest(
            "SPUS",
            10,
            75.00m,
            "USD");

        var result = await service.UpdateHoldingAsync(
            holding.Id,
            request);

        Assert.True(result);

        var updatedHolding = await dbContext.Holdings
            .SingleAsync(h => h.Id == holding.Id);

        Assert.Equal("SPUS", updatedHolding.Symbol);
        Assert.Equal(10, updatedHolding.Quantity);
        Assert.Equal(75.00m, updatedHolding.AverageCost);
        Assert.Equal("USD", updatedHolding.Currency);
        Assert.Equal(account.Id, updatedHolding.AccountId);
    }

    [Fact]
    public async Task UpdateHoldingAsync_WhenHoldingDoesNotExist_ReturnsFalse()
    {
        await using var dbContext = CreateDbContext();

        var service = new HoldingService(dbContext);

        var request = new UpdateHoldingRequest(
            "SPUS",
            10,
            75.00m,
            "USD");

        var result = await service.UpdateHoldingAsync(
            999,
            request);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteHoldingAsync_WhenHoldingExists_DeletesHolding()
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

        var holding = new Holding
        {
            AccountId = account.Id,
            Symbol = "HLAL",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        };

        dbContext.Holdings.Add(holding);
        await dbContext.SaveChangesAsync();

        var service = new HoldingService(dbContext);

        var result = await service.DeleteHoldingAsync(holding.Id);

        Assert.True(result);

        var deletedHolding = await dbContext.Holdings
            .FindAsync(holding.Id);

        Assert.Null(deletedHolding);
    }

    [Fact]
    public async Task DeleteHoldingAsync_WhenHoldingDoesNotExist_ReturnsFalse()
    {
        await using var dbContext = CreateDbContext();

        var service = new HoldingService(dbContext);

        var result = await service.DeleteHoldingAsync(999);

        Assert.False(result);
    }
}
