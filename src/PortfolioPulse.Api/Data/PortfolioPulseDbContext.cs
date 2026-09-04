using Microsoft.EntityFrameworkCore;
using PortfolioPulse.Api.Models;

namespace PortfolioPulse.Api.Data;

public class PortfolioPulseDbContext(DbContextOptions<PortfolioPulseDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts => Set<Account>();

    public DbSet<Holding> Holdings => Set<Holding>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Holding>()
            .Property(h => h.Quantity)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Holding>()
            .Property(h => h.AverageCost)
            .HasPrecision(18, 4);
    }
}