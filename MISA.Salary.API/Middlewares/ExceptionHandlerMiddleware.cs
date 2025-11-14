using Microsoft.AspNetCore.Diagnostics;
using MISA.Salary.Contract.Exceptions;
using MISA.Salary.Contract.Shared;

namespace MISA.Salary.API.Middlewares;

public class ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, exception.Message);
        var statusCode = GetExceptionResponseStatusCode(exception);
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/json";
        var message = GetExceptionResponseMessage(exception) ?? "";
        var errorResponse = new Result(statusCode, false, new Error(message, exception.Message));
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
        return true;
    }

    private static int GetExceptionResponseStatusCode(Exception exception)
    {
        return exception switch
        {
            BadRequestException => 400,
            NotFoundException => 404,
            ValidationException => 400,
            _ => 500
        };
    }

    private static string GetExceptionResponseMessage(Exception exception)
    {
        return exception switch
        {
            BadRequestException => "Bad request",
            NotFoundException => "Not found",
            ValidationException => "Invalid model",
            ConfigurationException => "Configuration error",
            _ => "Internal server error"
        };
    }
}


