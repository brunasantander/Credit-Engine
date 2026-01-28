using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;

namespace CreditEngine.Infrastructure.Repositories;

public class InMemoryCreditDecisionRepository : ICreditDecisionRepository
{
    public async Task AddAsync(CreditDecision decision)
    {
        // por enquanto pode ser fake / mock
        await Task.CompletedTask;
    }
}