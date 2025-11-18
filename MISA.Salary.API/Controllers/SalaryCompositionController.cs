using Microsoft.AspNetCore.Mvc;
using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Application.UseCases.Interfaces;

namespace MISA.Salary.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SalaryCompositionController(ISalaryCompostionService salaryCompostionService) : ApiBaseController
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAllSalaryCompositions(int pageSize, int pageIndex)
    {
        var res = await salaryCompostionService.GetAllSalaryComposition(pageSize, pageIndex);
        return ProcessResult(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSalaryComposition([FromBody] SalaryCompositionCreateRequest request)
    {
        var res = await salaryCompostionService.CreateSalaryComposition(request);
        return ProcessResult(res);
    }
}
