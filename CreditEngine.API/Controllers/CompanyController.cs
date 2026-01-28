using Microsoft.AspNetCore.Mvc;
using CreditEngine.Application.Services;
using CreditEngine.Application.DTOs;

namespace CreditEngine.API.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController : ControllerBase
{
    private readonly CompanyService _service;

    public CompanyController(CompanyService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest request)
    {
        var companyId = await _service.CreateAsync(
            request.Cnpj,
            request.AnnualRevenue,
            request.Ebitda,
            request.TotalDebt,
            request.Cash
        );

        return CreatedAtAction(nameof(Create), new { id = companyId }, new { id = companyId });
    }
}
