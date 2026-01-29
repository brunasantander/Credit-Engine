using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;
using CreditEngine.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CreditEngine.Infrastructure.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly CreditEngineDbContext _context;

    public CompanyRepository(CreditEngineDbContext context)
    {
        _context = context;
    }

    public async Task<Company?> GetByIdAsync(Guid id)
        => await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);

    public async Task AddAsync(Company company)
    {
        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();
    }

    public async Task<Company?> GetByDocument(string cnpj)
    {
        return await _context.Companies.FirstOrDefaultAsync(x => x.Cnpj == cnpj);
    }
}
