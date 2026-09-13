using PortfolioPulse.Api.DTOs;

namespace PortfolioPulse.Api.Services;

public interface IHoldingService
{
    Task<IEnumerable<HoldingDto>> GetHoldingsAsync();

    Task<HoldingDto?> GetHoldingAsync(int id);

    Task<HoldingDto?> CreateHoldingAsync(CreateHoldingRequest request);

    Task<bool> UpdateHoldingAsync(int id, UpdateHoldingRequest request);

    Task<bool> DeleteHoldingAsync(int id);
}
