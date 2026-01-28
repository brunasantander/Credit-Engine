namespace CreditEngine.Domain.Entities;

public class CreditDecision
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CompanyId { get; private set; }
    public bool Approved { get; private set; }
    public decimal? InterestRate { get; private set; }
    public string Reason { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    protected CreditDecision() { }

    public CreditDecision(
        Guid companyId,
        bool approved,
        decimal? interestRate,
        string reason
    )
    {
        CompanyId = companyId;
        Approved = approved;
        InterestRate = interestRate;
        Reason = reason;
    }
}