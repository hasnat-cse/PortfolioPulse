namespace PortfolioPulse.Api.DTOs;

public record CreateAccountRequest(
    string Name,
    string Brokerage,
    string AccountType,
    string Currency = "CAD");