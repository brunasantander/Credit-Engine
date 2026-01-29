using Microsoft.AspNetCore.Mvc;
using CreditEngine.Application.Services;
using CreditEngine.Application.DTOs;
using CreditEngine.Domain.Rules;

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
        try
        {
            var id = await _service.CreateAsync(
                request.Cnpj,
                request.AnnualRevenue,
                request.Ebitda,
                request.TotalDebt,
                request.Cash
            );

            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var company = await _service.GetCompanyById(id);

            return Ok(company);
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("document/{cnpj}")]
    public async Task<IActionResult> GetByDocument(string cnpj)
    {
        try
        {
            var company = await _service.GetCompanyByDocument(cnpj);

            return Ok(company);
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyRequest request)
    {
        try
        {
            var company = await _service.UpdateCompany(request);

            return Ok(company);
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        try
        {
            await _service.DeleteCompany(id);

            return Ok();
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}
