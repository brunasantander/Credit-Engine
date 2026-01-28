using CreditEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditEngine.Infrastructure.Data.Mappings;

public class CreditDecisionMap : IEntityTypeConfiguration<CreditDecision>
{
    public void Configure(EntityTypeBuilder<CreditDecision> builder)
    {
        builder.ToTable("CreditDecisions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyId).IsRequired();
        builder.Property(x => x.Approved).IsRequired();
        builder.Property(x => x.InterestRate);
        builder.Property(x => x.Reason).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
    }
}
