using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.DTOs;

public static class HoldingMappings
{
    public static HoldingDto ToDto(this Holding holding)
    {
        return new HoldingDto(
            holding.Id,
            holding.AccountId,
            holding.Symbol,
            holding.Quantity,
            holding.AverageCost,
            holding.Currency);
    }
}