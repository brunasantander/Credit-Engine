using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Rules;
using CreditEngine.Domain.Repositories;

namespace CreditEngine.Application.Services;

public class CreditEvaluationService
{
    private readonly ICompanyRepository _companyRepository;
    private readonly ICreditDecisionRepository _decisionRepository;

    public CreditEvaluationService(
        ICompanyRepository companyRepository,
        ICreditDecisionRepository decisionRepositpry
    )
    {
        _companyRepository = companyRepository;
        _decisionRepository = decisionRepositpry;
    }

    public async Task<CreditDecision> EvaluateAsync(Guid companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        var leverage = company.Leverage();

        var approved = CreditPolicy.IsEligible(
            company.AnnualRevenue,
            company.Ebitda,
            leverage
        );

        decimal? rate = approved ? CalculateRate(company, leverage) : null;

        var decision = new CreditDecision(
            company.Id,
            approved,
            rate,
            approved ? "Aprovado conforme política vigente" : "Reprovado por política de crédito"
        );
        await _decisionRepository.AddAsync(decision);

        return decision;
    }

    private decimal CalculateRate(Company company, decimal leverage)
    {
        var rate = 1.5m;

        if (leverage > 2) rate += 0.5m;
        if (company.AnnualRevenue < 20_000_000) rate += 0.3m;

        return rate;
    }
}