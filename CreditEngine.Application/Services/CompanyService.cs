using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;
using CreditEngine.Domain.Rules;
using CreditEngine.Application.DTOs;

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
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new BusinessException("CNPJ inválido");
        if (annualRevenue < 0)
            throw new BusinessException("Receita anual não pode ser negativa");
        if (ebitda < 0)
            throw new BusinessException("EBITDA não pode ser negativo");
        if (totalDebt < 0)
            throw new BusinessException("Dívida total não pode ser negativa");
        if (cash < 0)
            throw new BusinessException("Cash não pode ser negativo");

        var exists = await _companyRepository.GetByDocument(cnpj);

        if (exists is not null) throw new BusinessException("Empresa já existe");

        var normalizedCnpj = new string([.. cnpj.Where(char.IsDigit)]);
        var company = new Company(
            normalizedCnpj,
            annualRevenue,
            ebitda,
            totalDebt,
            cash
        );

        await _companyRepository.AddAsync(company);

        return company.Id;
    }

    public async Task<Company> GetCompanyById(Guid id)
    {
        var company = await _companyRepository.GetByIdAsync(id) ?? throw new BusinessException("Empresa não existe");
        return company;
    }

    public async Task<Company> GetCompanyByDocument(string cnpj)
    {
        var company = await _companyRepository.GetByDocument(cnpj) ?? throw new BusinessException("Empresa não existe");
        return company;
    }

    public async Task<List<Company>> GetAllAsync()
    {
        return await _companyRepository.GetAllAsync();
    }
    public async Task<Company> UpdateCompany(UpdateCompanyRequest request)
    {
        var company = await _companyRepository.GetByIdAsync(request.Id)
         ?? throw new BusinessException("Empresa não existe");

        company.Update(
            request.AnnualRevenue,
            request.Ebitda,
            request.TotalDebt,
            request.Cash
        );

        await _companyRepository.UpdateAsync(company);

        return company;
    }

    public async Task DeleteCompany(Guid id)
    {
        var exists = await _companyRepository.GetByIdAsync(id) ?? throw new BusinessException("Empresa não existe");
        await _companyRepository.DeleteAsync(id);
        exists = await _companyRepository.GetByIdAsync(id);
        if (exists is not null) throw new BusinessException("Erro ao deletar empresa");
    }
}
