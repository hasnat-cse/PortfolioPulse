using PortfolioPulse.Api.DTOs;

namespace PortfolioPulse.Api.Services;

public interface IAccountService
{
    Task<IEnumerable<AccountDto>> GetAccountsAsync();

    Task<AccountDto?> GetAccountAsync(int id);

    Task<AccountDto> CreateAccountAsync(CreateAccountRequest request);

    Task<bool> UpdateAccountAsync(int id, UpdateAccountRequest request);

    Task<DeleteAccountResult> DeleteAccountAsync(int id);

    Task<IEnumerable<HoldingDto>?> GetAccountHoldingsAsync(int id);
}