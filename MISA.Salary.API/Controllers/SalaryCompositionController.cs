using Microsoft.AspNetCore.Mvc;
using MISA.Salary.Application.Commons.Models.SalaryComposition;
using MISA.Salary.Application.UseCases.Interfaces;
using MISA.Salary.Contract.Query;
using MISA.Salary.Domain.Enums;

namespace MISA.Salary.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SalaryCompositionController(ISalaryCompostionService salaryCompostionService) : ApiBaseController
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAllSalaryCompositions()
    {
        var res = await salaryCompostionService.GetAllSalaryCompositionsAsync();
        return ProcessResult(res);
    }

    [HttpGet("page")]
    public async Task<IActionResult> FilterCompositionPagination([FromQuery] SalaryCompositionParameter parameter)
    {
        var res = await salaryCompostionService.FilterSalaryCompositionPaginationAsync(parameter);
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

    [HttpDelete]
    public async Task<IActionResult> BulkDeleteSalaryCompositions([FromBody] IEnumerable<int> ids)
    {
        var res = await salaryCompostionService.BulkDeleteSalaryCompositions(ids);
        return ProcessResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSalaryComposition([FromRoute] int id)
    {
        var res = await salaryCompostionService.DeleteSalaryComposition(id);
        return ProcessResult(res);
    }

    [HttpPost("default-composition")]
    public async Task<IActionResult> CheckDefaultSalryComposition([FromBody] IEnumerable<int> ids)
    {
        var res = await salaryCompostionService.CheckDefaultComposition(ids);
        return ProcessResult(res);
    }

    [HttpPatch("status/{id}")]
    public async Task<IActionResult> UpdateSalaryCompositionStatus([FromRoute] int id, [FromBody] StatusUpdateRequest request)
    {
        var res = await salaryCompostionService.UpdateSalaryCompositionStatus(id, request.Status);
        return ProcessResult(res);
    }

    [HttpPatch("list-status")]
    public async Task<IActionResult> UpdateSalaryCompositionListStatus([FromBody] IEnumerable<int> ids, [FromQuery] Status status)
    {
        var res = await salaryCompostionService.UpdateListSalaryCompositionStatus(ids, status);
        return ProcessResult(res);
    }

    [HttpPost("from-system")]
    public async Task<IActionResult> CreateSalaryCompositionFromSystem([FromBody] IEnumerable<int> ids)
    {
        var res = await salaryCompostionService.CreateSalaryCompositionFromSystemAsync(ids);
        return ProcessResult(res);
    }
}
