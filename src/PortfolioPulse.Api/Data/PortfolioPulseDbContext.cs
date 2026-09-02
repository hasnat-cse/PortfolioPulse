using Microsoft.EntityFrameworkCore;

namespace PortfolioPulse.Api.Data;

public class PortfolioPulseDbContext(DbContextOptions<PortfolioPulseDbContext> options) : DbContext(options)
{
}