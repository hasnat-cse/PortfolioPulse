using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.DTOs;

public static class AccountMappings
{
    public static AccountDto ToDto(this Account account)
    {
        return new AccountDto(
            account.Id,
            account.Name,
            account.Brokerage,
            account.AccountType,
            account.Currency);
    }
}