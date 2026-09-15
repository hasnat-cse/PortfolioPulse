using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.Models;
using PortfolioPulse.Api.Tests.Infrastructure;

namespace PortfolioPulse.Api.Tests.Controllers;

public class AccountsControllerTests
{
    [Fact]
    public async Task GetAccounts_ReturnsOkWithAccounts()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        using (var scope = factory.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider
                .GetRequiredService<PortfolioPulseDbContext>();

            dbContext.Accounts.Add(new Account
            {
                Name = "Test Account",
                Brokerage = "Test Brokerage",
                AccountType = "TFSA",
                Currency = "CAD"
            });

            await dbContext.SaveChangesAsync();
        }

        var response = await client.GetAsync("/api/accounts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var accounts = await response.Content
            .ReadFromJsonAsync<List<AccountResponse>>();

        Assert.NotNull(accounts);
        Assert.Single(accounts);

        Assert.Equal("Test Account", accounts[0].Name);
        Assert.Equal("Test Brokerage", accounts[0].Brokerage);
        Assert.Equal("TFSA", accounts[0].AccountType);
        Assert.Equal("CAD", accounts[0].Currency);
    }

    [Fact]
    public async Task GetAccount_WhenAccountExists_ReturnsOkWithAccount()
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
                Brokerage = "Test Brokerage",
                AccountType = "TFSA",
                Currency = "CAD"
            };

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            accountId = account.Id;
        }

        var response = await client.GetAsync($"/api/accounts/{accountId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var accountResponse = await response.Content
            .ReadFromJsonAsync<AccountResponse>();

        Assert.NotNull(accountResponse);

        Assert.Equal(accountId, accountResponse.Id);
        Assert.Equal("Test Account", accountResponse.Name);
        Assert.Equal("Test Brokerage", accountResponse.Brokerage);
        Assert.Equal("TFSA", accountResponse.AccountType);
        Assert.Equal("CAD", accountResponse.Currency);
    }

    [Fact]
    public async Task GetAccount_WhenAccountDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var response = await client.GetAsync("/api/accounts/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
        Assert.Equal("Account not found", problem.Title);
        Assert.Equal(
            "The specified account does not exist.",
            problem.Detail);
    }

    [Fact]
    public async Task CreateAccount_WithValidRequest_ReturnsCreated()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var request = new
        {
            Name = "New Account",
            Brokerage = "Questrade",
            AccountType = "FHSA",
            Currency = "CAD"
        };

        var response = await client.PostAsJsonAsync(
            "/api/accounts",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        Assert.NotNull(response.Headers.Location);
        Assert.Contains("/api/Accounts/", response.Headers.Location.ToString());

        var account = await response.Content
            .ReadFromJsonAsync<AccountResponse>();

        Assert.NotNull(account);

        Assert.True(account.Id > 0);
        Assert.Equal("New Account", account.Name);
        Assert.Equal("Questrade", account.Brokerage);
        Assert.Equal("FHSA", account.AccountType);
        Assert.Equal("CAD", account.Currency);
    }

    [Fact]
    public async Task CreateAccount_WithInvalidRequest_ReturnsBadRequestProblemDetails()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var request = new
        {
            Name = "",
            Brokerage = "Questrade",
            AccountType = "FHSA",
            Currency = "CAD"
        };

        var response = await client.PostAsJsonAsync(
            "/api/accounts",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content
            .ReadFromJsonAsync<ValidationProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(400, problem.Status);
        Assert.NotNull(problem.Errors);
        Assert.True(problem.Errors.ContainsKey("Name"));
    }

    [Fact]
    public async Task UpdateAccount_WhenAccountExists_ReturnsNoContent()
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
                Name = "Original Account",
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
            Name = "Updated Account",
            Brokerage = "Wealthsimple",
            AccountType = "FHSA",
            Currency = "CAD"
        };

        var response = await client.PutAsJsonAsync(
            $"/api/accounts/{accountId}",
            request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task UpdateAccount_WhenAccountDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var request = new
        {
            Name = "Updated Account",
            Brokerage = "Questrade",
            AccountType = "FHSA",
            Currency = "CAD"
        };

        var response = await client.PutAsJsonAsync(
            "/api/accounts/999",
            request);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
        Assert.Equal("Account not found", problem.Title);
        Assert.Equal(
            "The specified account does not exist.",
            problem.Detail);
    }

    [Fact]
    public async Task DeleteAccount_WhenAccountExistsWithoutHoldings_ReturnsNoContent()
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
                Name = "Account To Delete",
                Brokerage = "Questrade",
                AccountType = "TFSA",
                Currency = "CAD"
            };

            dbContext.Accounts.Add(account);
            await dbContext.SaveChangesAsync();

            accountId = account.Id;
        }

        var response = await client.DeleteAsync(
            $"/api/accounts/{accountId}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteAccount_WhenAccountDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var response = await client.DeleteAsync("/api/accounts/999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
        Assert.Equal("Account not found", problem.Title);
        Assert.Equal(
            "The specified account does not exist.",
            problem.Detail);
    }

    [Fact]
    public async Task DeleteAccount_WhenAccountHasHoldings_ReturnsConflictProblemDetails()
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
                Name = "Account With Holdings",
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

        var response = await client.DeleteAsync(
            $"/api/accounts/{accountId}");

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(409, problem.Status);
        Assert.Equal("Account cannot be deleted", problem.Title);
        Assert.Equal(
            "Cannot delete an account that has holdings.",
            problem.Detail);
    }

    [Fact]
    public async Task GetAccountHoldings_WhenAccountExists_ReturnsOkWithHoldings()
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

            dbContext.Holdings.AddRange(
                new Holding
                {
                    AccountId = accountId,
                    Symbol = "HLAL",
                    Quantity = 25,
                    AverageCost = 95.42m,
                    Currency = "USD"
                },
                new Holding
                {
                    AccountId = accountId,
                    Symbol = "SPSK",
                    Quantity = 10,
                    AverageCost = 20.50m,
                    Currency = "USD"
                });

            await dbContext.SaveChangesAsync();
        }

        var response = await client.GetAsync(
            $"/api/accounts/{accountId}/holdings");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var holdings = await response.Content
            .ReadFromJsonAsync<List<HoldingResponse>>();

        Assert.NotNull(holdings);
        Assert.Equal(2, holdings.Count);

        Assert.Contains(holdings, holding =>
            holding.Symbol == "HLAL" &&
            holding.Quantity == 25 &&
            holding.AverageCost == 95.42m &&
            holding.Currency == "USD");

        Assert.Contains(holdings, holding =>
            holding.Symbol == "SPSK" &&
            holding.Quantity == 10 &&
            holding.AverageCost == 20.50m &&
            holding.Currency == "USD");

        Assert.All(holdings, holding =>
            Assert.Equal(accountId, holding.AccountId));
    }

    [Fact]
    public async Task GetAccountHoldings_WhenAccountDoesNotExist_ReturnsNotFoundProblemDetails()
    {
        await using var factory = new CustomWebApplicationFactory();

        var client = factory.CreateClient();

        var response = await client.GetAsync(
            "/api/accounts/999/holdings");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        Assert.Equal(
            "application/problem+json",
            response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content
            .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(404, problem.Status);
        Assert.Equal("Account not found", problem.Title);
        Assert.Equal(
            "The specified account does not exist.",
            problem.Detail);
    }

    private sealed record AccountResponse(
        int Id,
        string Name,
        string Brokerage,
        string AccountType,
        string Currency);

    private sealed record HoldingResponse(
        int Id,
        int AccountId,
        string Symbol,
        decimal Quantity,
        decimal AverageCost,
        string Currency);
}
