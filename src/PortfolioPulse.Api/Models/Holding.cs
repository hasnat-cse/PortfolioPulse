using System.Text.Json.Serialization;

namespace PortfolioPulse.Api.Models;

public class Holding
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal AverageCost { get; set; }
    public string Currency { get; set; } = "CAD";

    [JsonIgnore]
    public Account? Account { get; set; }
}