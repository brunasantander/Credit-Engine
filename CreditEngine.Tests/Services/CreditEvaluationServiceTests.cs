using CreditEngine.Application.Services;
using CreditEngine.Domain.Entities;
using CreditEngine.Domain.Repositories;
using FluentAssertions;
using Moq;
using Xunit;

public class CreditEvaluationServiceTests
{
    [Fact]
    public async Task Should_Approve_Credit()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = new Company(
            cnpj: "12.345.678/0001-90",
            annualRevenue: 6_000_000,
            ebitda: 500_000,
            totalDebt: 1_000_000, // leverage = 2
            cash: 200_000
        );

        var companyRepository = new Mock<ICompanyRepository>();
        companyRepository
            .Setup(r => r.GetByIdAsync(companyId))
            .ReturnsAsync(company);

        var decisionRepository = new Mock<ICreditDecisionRepository>();

        var service = new CreditEvaluationService(
            companyRepository.Object,
            decisionRepository.Object
        );

        // Act
        var decision = await service.EvaluateAsync(companyId);

        // Assert
        decision.Approved.Should().BeTrue();
        decision.Reason.Should().Be("Aprovado conforme política vigente");

        decisionRepository.Verify(
            r => r.AddAsync(It.IsAny<CreditDecision>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Reject_Credit_When_Leverage_Is_High()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = new Company(
            cnpj: "98.765.432/0001-10",
            annualRevenue: 500_000,
            ebitda: 100_000,
            totalDebt: 600_000, // leverage = 6
            cash: 50_000
        );

        var companyRepository = new Mock<ICompanyRepository>();
        companyRepository
            .Setup(r => r.GetByIdAsync(companyId))
            .ReturnsAsync(company);

        var decisionRepository = new Mock<ICreditDecisionRepository>();

        var service = new CreditEvaluationService(
            companyRepository.Object,
            decisionRepository.Object
        );

        // Act
        var decision = await service.EvaluateAsync(companyId);

        // Assert
        decision.Approved.Should().BeFalse();
        decision.Reason.Should().Be("Reprovado por política de crédito");

        decisionRepository.Verify(
            r => r.AddAsync(It.IsAny<CreditDecision>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Reject_Credit_When_annualRevenue_Is_Lower_Than_5_000_000()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = new Company(
            cnpj: "98.765.432/0001-10",
            annualRevenue: 500_000,
            ebitda: 500_000,
            totalDebt: 1_000_000, // leverage = 6
            cash: 50_000
        );

        var companyRepository = new Mock<ICompanyRepository>();
        companyRepository
            .Setup(r => r.GetByIdAsync(companyId))
            .ReturnsAsync(company);

        var decisionRepository = new Mock<ICreditDecisionRepository>();

        var service = new CreditEvaluationService(
            companyRepository.Object,
            decisionRepository.Object
        );

        // Act
        var decision = await service.EvaluateAsync(companyId);

        // Assert
        decision.Approved.Should().BeFalse();
        decision.Reason.Should().Be("Reprovado por política de crédito");

        decisionRepository.Verify(
            r => r.AddAsync(It.IsAny<CreditDecision>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Reject_Credit_When_ebitda_Is_0()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = new Company(
            cnpj: "98.765.432/0001-10",
            annualRevenue: 500_000,
            ebitda: 0,
            totalDebt: 1_000_000,
            cash: 50_000
        );

        var companyRepository = new Mock<ICompanyRepository>();
        companyRepository
            .Setup(r => r.GetByIdAsync(companyId))
            .ReturnsAsync(company);

        var decisionRepository = new Mock<ICreditDecisionRepository>();

        var service = new CreditEvaluationService(
            companyRepository.Object,
            decisionRepository.Object
        );

        // Act
        var decision = await service.EvaluateAsync(companyId);

        // Assert
        decision.Approved.Should().BeFalse();
        decision.Reason.Should().Be("Reprovado por política de crédito");

        decisionRepository.Verify(
            r => r.AddAsync(It.IsAny<CreditDecision>()),
            Times.Once
        );
    }

    [Fact]
    public async Task Should_Reject_Credit_When_ebitda_Is_Lower_Than_0()
    {
        // Arrange
        var companyId = Guid.NewGuid();

        var company = new Company(
            cnpj: "98.765.432/0001-10",
            annualRevenue: 500_000,
            ebitda: -500_000,
            totalDebt: 1_000_000,
            cash: 50_000
        );

        var companyRepository = new Mock<ICompanyRepository>();
        companyRepository
            .Setup(r => r.GetByIdAsync(companyId))
            .ReturnsAsync(company);

        var decisionRepository = new Mock<ICreditDecisionRepository>();

        var service = new CreditEvaluationService(
            companyRepository.Object,
            decisionRepository.Object
        );

        // Act
        var decision = await service.EvaluateAsync(companyId);

        // Assert
        decision.Approved.Should().BeFalse();
        decision.Reason.Should().Be("Reprovado por política de crédito");

        decisionRepository.Verify(
            r => r.AddAsync(It.IsAny<CreditDecision>()),
            Times.Once
        );
    }
}
