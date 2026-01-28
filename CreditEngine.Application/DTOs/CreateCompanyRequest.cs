namespace CreditEngine.Application.DTOs;

public class CreateCompanyRequest
{
    public string Cnpj { get; set; } = default!;
    public decimal AnnualRevenue { get; set; }
    public decimal Ebitda { get; set; }
    public decimal TotalDebt { get; set; }
    public decimal Cash { get; set; }
}