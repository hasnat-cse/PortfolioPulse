namespace PortfolioPulse.Api.DTOs;

public record CreateHoldingRequest(
    int AccountId,
    string Symbol,
    decimal Quantity,
    decimal AverageCost,
    string Currency = "CAD");