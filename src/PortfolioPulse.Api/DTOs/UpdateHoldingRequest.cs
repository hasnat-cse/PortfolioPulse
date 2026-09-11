using System.ComponentModel.DataAnnotations;

namespace PortfolioPulse.Api.DTOs;

public record UpdateHoldingRequest(
    [Required]
    [StringLength(20)]
    string Symbol,

    [Range(0.0001, double.MaxValue)]
    decimal Quantity,

    [Range(0, double.MaxValue)]
    decimal AverageCost,

    [Required]
    [StringLength(3, MinimumLength = 3)]
    string Currency);