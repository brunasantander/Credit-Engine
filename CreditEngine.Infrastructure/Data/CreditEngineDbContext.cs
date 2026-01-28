using CreditEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CreditEngine.Infrastructure.Data;

public class CreditEngineDbContext : DbContext
{
    public CreditEngineDbContext(DbContextOptions<CreditEngineDbContext> options)
        : base(options) { }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CreditDecision> CreditDecisions => Set<CreditDecision>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Company>().HasData(
            new
            {
                Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Cnpj = "12.345.678/0001-90",
                AnnualRevenue = 1_800_000m,
                Ebitda = 360_000m,
                TotalDebt = 500_000m,
                Cash = 200_000m
            },
            new
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Cnpj = "98.765.432/0001-10",
                AnnualRevenue = 600_000m,
                Ebitda = 90_000m,
                TotalDebt = 150_000m,
                Cash = 50_000m
            },
            new
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Cnpj = "45.987.321/0001-55",
                AnnualRevenue = 3_600_000m,
                Ebitda = 900_000m,
                TotalDebt = 1_200_000m,
                Cash = 400_000m
            }
        );
    }

}
