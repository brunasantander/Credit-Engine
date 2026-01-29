using CreditEngine.Application.Services;
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
}
