namespace CreditEngine.Domain.Entities;

public class Company
{
    public Guid Id { get; private set; } = new Guid();
    public string Cnpj { get; private set; } = default!;
    public decimal AnnualRevenue { get; private set; }
    public decimal Ebitda { get; private set; }
    public decimal TotalDebt { get; private set; }
    public decimal Cash { get; private set; }

    protected Company() { }

    public Company(
        string cnpj,
        decimal annualRevenue,
        decimal ebitda,
        decimal totalDebt,
        decimal cash
    )
    {
        Id = Guid.NewGuid();
        Cnpj = cnpj;
        AnnualRevenue = annualRevenue;
        Ebitda = ebitda;
        TotalDebt = totalDebt;
        Cash = cash;
    }

    public decimal Leverage()
    {
        if (Ebitda <= 0) return decimal.MaxValue;
        return TotalDebt / Ebitda;
    }

    public void Update(
    decimal? annualRevenue,
    decimal? ebitda,
    decimal? totalDebt,
    decimal? cash)
    {
        if (annualRevenue.HasValue)
            AnnualRevenue = annualRevenue.Value;

        if (ebitda.HasValue)
            Ebitda = ebitda.Value;

        if (totalDebt.HasValue)
            TotalDebt = totalDebt.Value;

        if (cash.HasValue)
            Cash = cash.Value;
    }
}