using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.Models;
using PortfolioPulse.Api.Tests.Infrastructure;

namespace PortfolioPulse.Api.Tests.Controllers;

public class HoldingsControllerTests
{
    [Fact]
    public async Task GetHoldings_ReturnsOkWithHoldings()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        int accountId;

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<PortfolioPulseDbContext>();

            var account = new Account
            {
                Name = "Test Account",
                Brokerage = "Questrade",
                AccountType = "TFSA",
                Currency = "CAD"
            };

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            accountId = account.Id;

            dbContext.Holdings.Add(new Holding
            {
                AccountId = accountId,
                Symbol = "HLAL",
                Quantity = 25,
                AverageCost = 95.42m,
                Currency = "USD"
            });

            await dbContext.SaveChangesAsync();
        }

        var response = await client.GetAsync("/api/Holdings");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var holdings = await response.Content
            .ReadFromJsonAsync<List<HoldingResponse>>();

        Assert.NotNull(holdings);
        Assert.Single(holdings);

        Assert.Equal(accountId, holdings[0].AccountId);
        Assert.Equal("HLAL", holdings[0].Symbol);
        Assert.Equal(25, holdings[0].Quantity);
        Assert.Equal(95.42m, holdings[0].AverageCost);
        Assert.Equal("USD", holdings[0].Currency);
    }

    [Fact]
    public async Task GetHolding_WhenHoldingExists_ReturnsOkWithHolding()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        int holdingId;
        int accountId;

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<PortfolioPulseDbContext>();

            var account = new Account
            {
                Name = "Test Account",
                Brokerage = "Questrade",
                AccountType = "TFSA",
                Currency = "CAD"
            };

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            accountId = account.Id;

            var holding = new Holding
            {
                AccountId = accountId,
                Symbol = "HLAL",
                Quantity = 25,
                AverageCost = 95.42m,
                Currency = "USD"
            };

            dbContext.Holdings.Add(holding);
            await dbContext.SaveChangesAsync();

            holdingId = holding.Id;
        }

        var response = await client.GetAsync(
            $"/api/Holdings/{holdingId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var holdingResponse = await response.Content
            .ReadFromJsonAsync<HoldingResponse>();

        Assert.NotNull(holdingResponse);

        Assert.Equal(holdingId, holdingResponse.Id);
        Assert.Equal(accountId, holdingResponse.AccountId);
        Assert.Equal("HLAL", holdingResponse.Symbol);
        Assert.Equal(25, holdingResponse.Quantity);
        Assert.Equal(95.42m, holdingResponse.AverageCost);
        Assert.Equal("USD", holdingResponse.Currency);
    }

    [Fact]
    public async Task GetHolding_WhenHoldingDoesNotExist_ReturnsNotFound()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/Holdings/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateHolding_WithValidRequest_ReturnsCreated()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        int accountId;

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<PortfolioPulseDbContext>();

            var account = new Account
            {
                Name = "Test Account",
                Brokerage = "Questrade",
                AccountType = "TFSA",
                Currency = "CAD"
            };

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            accountId = account.Id;
        }

        var request = new
        {
            AccountId = accountId,
            Symbol = "HLAL",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        };

        var response = await client.PostAsJsonAsync(
            "/api/Holdings",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.NotNull(response.Headers.Location);
        Assert.Contains(
            "/api/Holdings/",
            response.Headers.Location.ToString());

        var holding = await response.Content
            .ReadFromJsonAsync<HoldingResponse>();

        Assert.NotNull(holding);

        Assert.True(holding.Id > 0);
        Assert.Equal(accountId, holding.AccountId);
        Assert.Equal("HLAL", holding.Symbol);
        Assert.Equal(25, holding.Quantity);
        Assert.Equal(95.42m, holding.AverageCost);
        Assert.Equal("USD", holding.Currency);
    }

    [Fact]
    public async Task CreateHolding_WhenAccountDoesNotExist_ReturnsBadRequest()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var request = new
        {
            AccountId = 999,
            Symbol = "HLAL",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        };

        var response = await client.PostAsJsonAsync(
            "/api/Holdings",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateHolding_WithInvalidRequest_ReturnsBadRequest()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var request = new
        {
            AccountId = 1,
            Symbol = "",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        };

        var response = await client.PostAsJsonAsync(
            "/api/Holdings",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateHolding_WhenHoldingExists_ReturnsNoContent()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        int holdingId;
        int accountId;

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<PortfolioPulseDbContext>();

            var account = new Account
            {
                Name = "Test Account",
                Brokerage = "Questrade",
                AccountType = "TFSA",
                Currency = "CAD"
            };

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            accountId = account.Id;

            var holding = new Holding
            {
                AccountId = accountId,
                Symbol = "HLAL",
                Quantity = 25,
                AverageCost = 95.42m,
                Currency = "USD"
            };

            dbContext.Holdings.Add(holding);
            await dbContext.SaveChangesAsync();

            holdingId = holding.Id;
        }

        var request = new
        {
            Symbol = "SPSK",
            Quantity = 10,
            AverageCost = 20.50m,
            Currency = "USD"
        };

        var response = await client.PutAsJsonAsync(
            $"/api/Holdings/{holdingId}",
            request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateHolding_WhenHoldingDoesNotExist_ReturnsNotFound()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var request = new
        {
            Symbol = "HLAL",
            Quantity = 25,
            AverageCost = 95.42m,
            Currency = "USD"
        };

        var response = await client.PutAsJsonAsync(
            "/api/Holdings/999",
            request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteHolding_WhenHoldingExists_ReturnsNoContent()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        int holdingId;
        int accountId;

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<PortfolioPulseDbContext>();

            var account = new Account
            {
                Name = "Test Account",
                Brokerage = "Questrade",
                AccountType = "TFSA",
                Currency = "CAD"
            };

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            accountId = account.Id;

            var holding = new Holding
            {
                AccountId = accountId,
                Symbol = "HLAL",
                Quantity = 25,
                AverageCost = 95.42m,
                Currency = "USD"
            };

            dbContext.Holdings.Add(holding);
            await dbContext.SaveChangesAsync();

            holdingId = holding.Id;
        }

        var response = await client.DeleteAsync(
            $"/api/Holdings/{holdingId}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteHolding_WhenHoldingDoesNotExist_ReturnsNotFound()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/Holdings/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private sealed record HoldingResponse(
        int Id,
        int AccountId,
        string Symbol,
        decimal Quantity,
        decimal AverageCost,
        string Currency);
}
