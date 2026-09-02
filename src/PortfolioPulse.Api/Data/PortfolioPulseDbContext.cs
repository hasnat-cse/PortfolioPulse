using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Data;

public class PortfolioPulseDbContext(DbContextOptions<PortfolioPulseDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();
}