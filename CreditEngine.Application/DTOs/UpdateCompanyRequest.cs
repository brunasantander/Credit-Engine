namespace CreditEngine.Application.DTOs;

public class UpdateCompanyRequest
{
    public Guid Id { get; set; }
    public decimal? AnnualRevenue { get; set; }
    public decimal? Ebitda { get; set; }
    public decimal? TotalDebt { get; set; }
    public decimal? Cash { get; set; }
}