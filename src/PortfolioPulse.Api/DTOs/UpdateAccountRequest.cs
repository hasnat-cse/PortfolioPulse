namespace PortfolioPulse.Api.DTOs;

public record UpdateAccountRequest(
    string Name,
    string Brokerage,
    string AccountType,
    string Currency);