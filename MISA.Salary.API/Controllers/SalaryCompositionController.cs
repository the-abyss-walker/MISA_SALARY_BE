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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSalaryCompositionById([FromRoute] int id)
    {
        var res = await salaryCompostionService.GetSalaryCompositionById(id);
        return ProcessResult(res);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSalaryComposition([FromBody] SalaryCompositionCreateRequest request)
    {
        var res = await salaryCompostionService.CreateSalaryComposition(request);
        return ProcessResult(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSalaryComposition([FromBody] SalaryCompositionUpdateRequest request)
    {
        var res = await salaryCompostionService.UpdateSalaryComposition(request);
        return ProcessResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalaryComposition([FromRoute] int id)
    {
        var res = await salaryCompostionService.DeleteSalaryComposition(id);
        return ProcessResult(res);
    }

    [HttpDelete("bulk-delete")]
    public async Task<IActionResult> BulkDeleteSalaryCompositions([FromBody] IEnumerable<int> ids)
    {
        var res = await salaryCompostionService.BulkDeleteSalaryCompositions(ids);
        return ProcessResult(res);
    }
}
