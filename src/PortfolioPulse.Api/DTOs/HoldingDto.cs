namespace PortfolioPulse.Api.DTOs;

public record HoldingDto(
    int Id,
    int AccountId,
    string Symbol,
    decimal Quantity,
    decimal AverageCost,
    string Currency);