namespace PortfolioPulse.Api.DTOs;

public record UpdateHoldingRequest(
    string Symbol,
    decimal Quantity,
    decimal AverageCost,
    string Currency);