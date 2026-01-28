using Microsoft.AspNetCore.Mvc;
using CreditEngine.Application.Services;

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
    public async Task<IActionResult> Evaluate([FromBody] CreditEvaluationRequestDto dto)
    {
        var result = await _service.EvaluateAsync(dto.CompanyId);
        return Ok(result);
    }
}