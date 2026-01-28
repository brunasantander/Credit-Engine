using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;

namespace CreditEngine.Application.Services;

public class CompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }

    public async Task<Guid> CreateAsync(
        string cnpj,
        decimal annualRevenue,
        decimal ebitda,
        decimal totalDebt,
        decimal cash)
    {
        var company = new Company(
            cnpj,
            annualRevenue,
            ebitda,
            totalDebt,
            cash
        );

        await _companyRepository.AddAsync(company);

        return company.Id;
    }
}
