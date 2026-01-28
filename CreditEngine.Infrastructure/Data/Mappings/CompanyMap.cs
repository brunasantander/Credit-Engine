using CreditEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditEngine.Infrastructure.Data.Mappings;

public class CompanyMap : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Companies");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Cnpj).IsRequired();
        builder.Property(x => x.AnnualRevenue).IsRequired();
        builder.Property(x => x.Ebitda).IsRequired();
        builder.Property(x => x.TotalDebt).IsRequired();
        builder.Property(x => x.Cash).IsRequired();
    }
}