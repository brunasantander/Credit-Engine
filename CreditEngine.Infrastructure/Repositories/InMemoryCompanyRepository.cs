using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;

namespace CreditEngine.Infrastructure.Repositories;

public class InMemoryCompanyRepository : ICompanyRepository
{
    private static readonly List<Company> _companies = new();

    public Task AddAsync(Company company)
    {
        _companies.Add(company);
        return Task.CompletedTask;
    }

    public Task<Company> GetByIdAsync(Guid id)
    {
        var company = _companies.First(c => c.Id == id);
        return Task.FromResult(company);
    }
}
