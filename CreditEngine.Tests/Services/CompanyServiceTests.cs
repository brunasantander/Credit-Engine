using CreditEngine.Application.Services;
using CreditEngine.Application.DTOs;
using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;
using CreditEngine.Domain.Rules;
using FluentAssertions;
using Moq;
using Xunit;

public class CompanyServiceTests
{
    [Fact]
    public async Task Should_Create_Company()
    {
        var companyRepository = new Mock<ICompanyRepository>();

        var service = new CompanyService(
            companyRepository.Object
        );

        // Act
        var companyId = await service.CreateAsync(
            cnpj: "12.345.678/0001-90",
            annualRevenue: 6_000_000,
            ebitda: 500_000,
            totalDebt: 1_000_000,
            cash: 200_000
        );

        // Assert
        companyId.Should().NotBe(Guid.Empty);

        companyRepository.Verify(
            r => r.AddAsync(It.IsAny<Company>()),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Cnpj_Is_Empty()
    {
        // Arrange
        var companyRepository = new Mock<ICompanyRepository>();
        var service = new CompanyService(companyRepository.Object);

        // Act
        Func<Task> act = async () =>
        {
            await service.CreateAsync(
                cnpj: "",          // cnpj inválido
                annualRevenue: 1_000_000,
                ebitda: 500_000,
                totalDebt: 100_000,
                cash: 50_000
            );
        };

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("CNPJ inválido");

        // Garantir que o repositório **não foi chamado**
        companyRepository.Verify(r => r.AddAsync(It.IsAny<Company>()), Times.Never);
    }


    [Fact]
    public async Task CreateAsync_Should_Throw_When_annualRevenue_Is_Lower_Than_0()
    {
        // Arrange
        var companyRepository = new Mock<ICompanyRepository>();
        var service = new CompanyService(companyRepository.Object);

        // Act
        Func<Task> act = async () =>
        {
            await service.CreateAsync(
                cnpj: "12.345.678/0001-90",
                annualRevenue: -1_000_000,
                ebitda: 500_000,
                totalDebt: 100_000,
                cash: 50_000
            );
        };

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Receita anual não pode ser negativa");

        // Garantir que o repositório **não foi chamado**
        companyRepository.Verify(r => r.AddAsync(It.IsAny<Company>()), Times.Never);
    }


    [Fact]
    public async Task CreateAsync_Should_Throw_When_ebitda_Is_Lower_Than_0()
    {
        // Arrange
        var companyRepository = new Mock<ICompanyRepository>();
        var service = new CompanyService(companyRepository.Object);

        // Act
        Func<Task> act = async () =>
        {
            await service.CreateAsync(
                cnpj: "12.345.678/0001-90",
                annualRevenue: 1_000_000,
                ebitda: -500_000,
                totalDebt: 100_000,
                cash: 50_000
            );
        };

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("EBITDA não pode ser negativo");

        // Garantir que o repositório **não foi chamado**
        companyRepository.Verify(r => r.AddAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_totalDebt_Is_Lower_Than_0()
    {
        // Arrange
        var companyRepository = new Mock<ICompanyRepository>();
        var service = new CompanyService(companyRepository.Object);

        // Act
        Func<Task> act = async () =>
        {
            await service.CreateAsync(
                cnpj: "12.345.678/0001-90",
                annualRevenue: 1_000_000,
                ebitda: 500_000,
                totalDebt: -100_000,
                cash: 50_000
            );
        };

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Dívida total não pode ser negativa");

        // Garantir que o repositório **não foi chamado**
        companyRepository.Verify(r => r.AddAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_cash_Is_Lower_Than_0()
    {
        // Arrange
        var companyRepository = new Mock<ICompanyRepository>();
        var service = new CompanyService(companyRepository.Object);

        // Act
        Func<Task> act = async () =>
        {
            await service.CreateAsync(
                cnpj: "12.345.678/0001-90",
                annualRevenue: 1_000_000,
                ebitda: 500_000,
                totalDebt: 100_000,
                cash: -50_000
            );
        };

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Cash não pode ser negativo");

        // Garantir que o repositório **não foi chamado**
        companyRepository.Verify(r => r.AddAsync(It.IsAny<Company>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Company_Already_Exists()
    {
        // Arrange
        var companyRepository = new Mock<ICompanyRepository>();

        var existingCompany = new Company(
            cnpj: "12.345.678/0001-90",
            annualRevenue: 1_000_000,
            ebitda: 500_000,
            totalDebt: 100_000,
            cash: 50_000
        );

        // Simula que a empresa já existe no repositório
        companyRepository
            .Setup(r => r.GetByDocument("12.345.678/0001-90"))
            .ReturnsAsync(existingCompany);

        var service = new CompanyService(companyRepository.Object);

        // Act
        Func<Task> act = async () => await service.CreateAsync(
            cnpj: "12.345.678/0001-90",
            annualRevenue: 6_000_000,
            ebitda: 500_000,
            totalDebt: 1_000_000,
            cash: 200_000
        );

        // Assert
        await act.Should()
            .ThrowAsync<BusinessException>()
            .WithMessage("Empresa já existe");

        // Verifica que o AddAsync **não foi chamado**
        companyRepository.Verify(r => r.AddAsync(It.IsAny<Company>()), Times.Never);
    }


    [Fact]
    public async Task UpdateCompany_Should_Update_When_Company_Exists()
    {
        // Arrange
        var companyRepository = new Mock<ICompanyRepository>();

        var existingCompany = new Company(
            cnpj: "12345678000190",
            annualRevenue: 1_000_000,
            ebitda: 500_000,
            totalDebt: 100_000,
            cash: 50_000
        );

        var request = new UpdateCompanyRequest
        {
            Id = existingCompany.Id,
            AnnualRevenue = 2_000_000,
            Ebitda = 800_000,
            TotalDebt = 200_000,
            Cash = 100_000
        };

        companyRepository
            .Setup(r => r.GetByIdAsync(existingCompany.Id))
            .ReturnsAsync(existingCompany);

        var service = new CompanyService(companyRepository.Object);

        // Act
        var updatedCompany = await service.UpdateCompany(request);

        // Assert
        updatedCompany.AnnualRevenue.Should().Be(2_000_000);
        updatedCompany.Ebitda.Should().Be(800_000);
        updatedCompany.TotalDebt.Should().Be(200_000);
        updatedCompany.Cash.Should().Be(100_000);

        companyRepository.Verify(
            r => r.UpdateAsync(existingCompany),
            Times.Once
        );
    }


    [Fact]
    public async Task DeleteCompany_Should_Delete_When_Exists()
    {
        var companyRepository = new Mock<ICompanyRepository>();

        var company = new Company(
            cnpj: "12345678000190",
            annualRevenue: 1_000_000,
            ebitda: 500_000,
            totalDebt: 100_000,
            cash: 50_000
        );

        companyRepository
            .SetupSequence(r => r.GetByIdAsync(company.Id))
            .ReturnsAsync(company)
            .ReturnsAsync((Company?)null);

        var service = new CompanyService(companyRepository.Object);

        await service.DeleteCompany(company.Id);

        companyRepository.Verify(
            r => r.DeleteAsync(company.Id),
            Times.Once
        );
    }
}
