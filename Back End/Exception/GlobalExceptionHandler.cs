using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;

namespace Back_End.ExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
    System.Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId
        );
        var (statusCode, title) = GetStatusCodeAndTitle(exception);

        var message = string.IsNullOrWhiteSpace(exception.Message) ? title : exception.Message;
        
        await Results.Problem(
            title: message,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                    {"traceId", traceId}
            }
        ).ExecuteAsync(httpContext);

        return true;
    }


    private static (int statusCode, string title) GetStatusCodeAndTitle(Exception exception)
    {
        return exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, "A validation error occurred"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "The specified resource was not found"),
            _ => (StatusCodes.Status500InternalServerError, "An unhandled error occurred")
        };
    }
}