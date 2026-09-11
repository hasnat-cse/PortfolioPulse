using System.ComponentModel.DataAnnotations;

namespace PortfolioPulse.Api.DTOs;

public record UpdateAccountRequest(
    [Required]
    [StringLength(100)]
    string Name,

    [Required]
    [StringLength(100)]
    string Brokerage,

    [Required]
    [StringLength(20)]
    string AccountType,

    [Required]
    [StringLength(3, MinimumLength = 3)]
    string Currency);