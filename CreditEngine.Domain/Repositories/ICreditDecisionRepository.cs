using CreditEngine.Domain.Entities;

namespace CreditEngine.Domain.Repositories;

public interface ICreditDecisionRepository
{
    Task AddAsync(CreditDecision decision);
}
