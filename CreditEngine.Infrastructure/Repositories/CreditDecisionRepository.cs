using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;
using CreditEngine.Infrastructure.Data;

namespace CreditEngine.Infrastructure.Repositories;

public class CreditDecisionRepository : ICreditDecisionRepository
{
    private readonly CreditEngineDbContext _context;

    public CreditDecisionRepository(CreditEngineDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CreditDecision decision)
    {
        await _context.CreditDecisions.AddAsync(decision);
        await _context.SaveChangesAsync();
    }
}
