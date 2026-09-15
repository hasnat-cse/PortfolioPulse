using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Data;
using PortfolioPulse.Api.DTOs;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Services;

public sealed class HoldingService(PortfolioPulseDbContext dbContext)
    : IHoldingService
{
    public async Task<IEnumerable<HoldingDto>> GetHoldingsAsync()
    {
        return await dbContext.Holdings
            .Select(HoldingMappings.ToDtoExpression)
            .ToListAsync();
    }

    public async Task<HoldingDto?> GetHoldingAsync(int id)
    {
        return await dbContext.Holdings
            .Where(holding => holding.Id == id)
            .Select(HoldingMappings.ToDtoExpression)
            .FirstOrDefaultAsync();
    }

    public async Task<HoldingDto?> CreateHoldingAsync(
        CreateHoldingRequest request)
    {
        var accountExists = await dbContext.Accounts
            .AnyAsync(account => account.Id == request.AccountId);

        if (!accountExists)
        {
            return null;
        }

        var holding = new Holding
        {
            AccountId = request.AccountId,
            Symbol = request.Symbol.Trim(),
            Quantity = request.Quantity,
            AverageCost = request.AverageCost,
            Currency = request.Currency
        };

        dbContext.Holdings.Add(holding);

        await dbContext.SaveChangesAsync();

        return holding.ToDto();
    }

    public async Task<bool> UpdateHoldingAsync(
        int id,
        UpdateHoldingRequest request)
    {
        var holding = await dbContext.Holdings.FindAsync(id);

        if (holding is null)
        {
            return false;
        }

        holding.Symbol = request.Symbol.Trim();
        holding.Quantity = request.Quantity;
        holding.AverageCost = request.AverageCost;
        holding.Currency = request.Currency;

        await dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteHoldingAsync(int id)
    {
        var holding = await dbContext.Holdings.FindAsync(id);

        if (holding is null)
        {
            return false;
        }

        dbContext.Holdings.Remove(holding);

        await dbContext.SaveChangesAsync();

        return true;
    }
}
