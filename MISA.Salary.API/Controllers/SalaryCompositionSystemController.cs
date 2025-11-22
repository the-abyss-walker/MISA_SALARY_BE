using Microsoft.AspNetCore.Mvc;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Query;

namespace MISA.Salary.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SalaryCompositionSystemController : ApiBaseController
{
    private readonly ISalaryCompositionSystemService _salaryCompositionSystemService;

    public SalaryCompositionSystemController(
        ISalaryCompositionSystemService salaryCompositionSystemService)
    {
        _salaryCompositionSystemService = salaryCompositionSystemService;
    }

    [HttpGet("page")]
    public async Task<IActionResult> FilterSalaryCompositionSystemPagination([FromQuery] SalaryCompositionSystemParameter parameter)
    {
        var res = await _salaryCompositionSystemService.FilterSalaryCompositionSystemPaginationAsync(parameter);
        return ProcessResult(res);
    }

    [HttpPost("exist-composition-code")]
    public async Task<IActionResult> ExistCompositionCode([FromQuery] string code)
    {
        var res = await _salaryCompositionSystemService.ExistCompositionCode(code);
        return ProcessResult(res);
    }
}
