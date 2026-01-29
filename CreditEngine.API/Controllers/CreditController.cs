using Microsoft.AspNetCore.Mvc;
using CreditEngine.Application.Services;
using Microsoft.VisualBasic;
using System.IO.Pipelines;
using CreditEngine.Application.DTOs;

namespace CreditEngine.API.Controllers;

[ApiController]
[Route("api/credit")]
public class CreditController : ControllerBase
{
    private readonly CreditEvaluationService _service;

    public CreditController(CreditEvaluationService service)
    {
        _service = service;
    }

    [HttpPost("evaluate")]
    public async Task<IActionResult> Evaluate([FromBody] CreditEvaluationRequest dto)
    {
        var result = await _service.EvaluateAsync(dto.CompanyId);
        if (result is null) return NotFound("Company not found");
        return Ok(result);
    }
}