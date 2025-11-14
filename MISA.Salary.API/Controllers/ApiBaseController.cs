using Microsoft.AspNetCore.Mvc;
using MISA.Salary.API.Attributes;
using MISA.Salary.Contract.Shared;

namespace MISA.Salary.API.Controllers;

[ValidationRequestModel]
[ApiController]
public abstract class ApiBaseController : ControllerBase
{
    protected IActionResult OnResultSuccess<T>(T result)
        where T : Result
        => result.StatusCode switch
        {
            StatusCodes.Status200OK => Ok(result),
            _ => StatusCode(result.StatusCode, result)
        };

    protected IActionResult OnResultFailure<T>(T result)
        where T : Result
        => result.StatusCode switch
        {
            StatusCodes.Status400BadRequest => BadRequest(result),
            StatusCodes.Status404NotFound => NotFound(result),
            _ => StatusCode(result.StatusCode, result)
        };

    protected IActionResult ProcessResult<T>(T result)
        where T : Result
    {
        if (result == null)
        {
            throw new BadHttpRequestException("Response api không được để trống");
        }
        return result.IsSuccess ? OnResultSuccess(result) : OnResultFailure(result);
    }
}
