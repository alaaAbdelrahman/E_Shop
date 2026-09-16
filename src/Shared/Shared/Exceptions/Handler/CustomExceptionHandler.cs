using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Shared.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger)
    : IExceptionHandler

{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message: {exceptionMessage},time of occurence {time}",
            exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) details = exception switch
        {
            InternalServerException => (exception.Message, exception.GetType().Name,
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError),

            NotFoundException => (exception.Message, exception.GetType().Name,
              httpContext.Response.StatusCode = StatusCodes.Status404NotFound),

            BadRequestException => (exception.Message, exception.GetType().Name,
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest),


            _ => (exception.Message, exception.GetType().Name,
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError)

        };

        var problemDetails = new ProblemDetails
        {
            Detail = details.Detail,
            Title = details.Title,
            Status = details.StatusCode,
            Instance = httpContext.Request.Path
        };
        problemDetails.Extensions.Add("traceId", httpContext.TraceIdentifier);

        if (exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("validationErrors", validationException.ValidationResult);
        }
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
