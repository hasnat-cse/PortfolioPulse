namespace PortfolioPulse.Api.DTOs;

public record AccountDto(
    int Id,
    string Name,
    string Brokerage,
    string AccountType,
    string Currency);