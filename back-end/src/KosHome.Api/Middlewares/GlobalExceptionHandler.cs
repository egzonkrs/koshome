using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentResults;
using KosHome.Api.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace KosHome.Api.Middlewares;

/// <summary>
/// Handles unhandled exceptions globally and returns a 500 Internal Server Error response.
/// </summary>
internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
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

        var problemDetails = new ApiResponse<Result>
        {
            IsFailed = true,
            IsSuccess = false,
            Errors = new Dictionary<string, string>
            {
                { "UNHANDLED_ERROR", exceptionThrew.Message }
            },
        };

        httpContext.Response.StatusCode = httpResponseStatus;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}