using Microsoft.AspNetCore.Mvc;
using MISA.Salary.Application.UseCases.Interfaces;

namespace MISA.Salary.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrganizationUnitController : ApiBaseController
{
    private readonly IOrganizationUnitService _organizationUnitService;

    public OrganizationUnitController(IOrganizationUnitService organizationUnitService)
    {
        _organizationUnitService = organizationUnitService;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllOrganizationUnits()
    {
        var res = await _organizationUnitService.GetAllOrganizationUnitsAsync();
        return ProcessResult(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrganizationUnitById([FromRoute] int id)
    {
        var res = await _organizationUnitService.GetOrganizationUnitByIdAsync(id);
        return ProcessResult(res);
    }
}
