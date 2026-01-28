using CreditEngine.Domain.Entities;

namespace CreditEngine.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company> GetByIdAsync(Guid id);
    Task AddAsync(Company company);
}
