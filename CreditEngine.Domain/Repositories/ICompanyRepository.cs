using System.Security.AccessControl;
using CreditEngine.Domain.Entities;

namespace CreditEngine.Domain.Repositories;

public interface ICompanyRepository
{
    Task<Company?> GetByIdAsync(Guid id);
    Task AddAsync(Company company);
    Task<Company?> GetByDocument(string cnpj);
    Task<List<Company>> GetAllAsync();
    Task UpdateAsync(Company company);
    Task DeleteAsync(Guid id);
}
