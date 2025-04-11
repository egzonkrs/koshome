using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KosHome.Api.Middlewares;

/// <summary>
/// Handles unhandled exceptions globally and returns a 500 Internal Server Error response.
/// </summary>
internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService)
    {
        _logger = logger;
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exceptionThrew, CancellationToken cancellationToken)
    {
        _logger.LogError(exceptionThrew, "Exception occurred: {Message}", exceptionThrew.Message);

        var httpResponseStatus = exceptionThrew switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };
        
        httpContext.Response.StatusCode = httpResponseStatus;

        var problemDetails = new ProblemDetails
        {
            Status = httpResponseStatus,
            Title = "An error occurred",
            Type = exceptionThrew.GetType().Name,
            Detail = exceptionThrew.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;
        
        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exceptionThrew,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}