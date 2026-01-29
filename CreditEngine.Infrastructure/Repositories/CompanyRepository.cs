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

    public async Task<List<Company>> GetAllAsync()
    {
        return await _context.Companies.ToListAsync();
    }

    public async Task UpdateAsync(Company company)
    {
        _context.Companies.Update(company);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var company = await GetByIdAsync(id);
        _context.Companies.Remove(company);
        await _context.SaveChangesAsync();
    }
}
