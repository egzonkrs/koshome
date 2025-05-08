using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentResults;
using KosHome.Api.Models;
using KosHome.Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KosHome.Api.Extensions;

/// <summary>
/// Extensions for converting FluentResults into ActionResult responses.
/// </summary>
public static class ControllerBaseExtensions
{
    /// <summary>
    /// Creates an ActionResult from a Result{T} by mapping it to an ApiResult{T}.
    /// </summary>
    /// <typeparam name="TData">The type of the value in the result.</typeparam>
    /// <param name="controller">The ControllerBase instance.</param>
    /// <param name="result">The FluentResults result.</param>
    /// <param name="successCode">The HTTP status code to use on success.</param>
    /// <returns>An ActionResult containing an ApiResult.</returns>
    public static IActionResult ToActionResult<TData>(this ControllerBase controller, Result<TData> result, HttpStatusCode successCode = HttpStatusCode.OK)
    {
        if (result.IsSuccess)
        {
            return controller.StatusCode((int)successCode, new ApiResponse<TData>
            {
                Data = result.ValueOrDefault,
                IsFailed = result.IsFailed,
                IsSuccess = result.IsSuccess,
                Reasons = result.Reasons.ToCodeMessageDictionary(),
                Errors = result.Errors.ToCodeMessageDictionary()
            });
        }
        
        if (result.IsFailed && result.HasException<Exception>())
        {
            var problemDetails = new ApiResponse<TData>
            {
                IsFailed = result.IsFailed,
                IsSuccess = result.IsSuccess,
                Errors = result.Errors.ToCodeMessageDictionary(),
            };
            
            return controller.StatusCode((int)HttpStatusCode.InternalServerError, problemDetails);
        }

        var isUnauthorized = result.Errors.Any(err =>
            err is CustomFluentError customError && customError.Code.Equals("INVALID_CREDENTIALS", StringComparison.OrdinalIgnoreCase));

        if (isUnauthorized)
        {
            return controller.Unauthorized(new ApiResponse<TData>
            {
                IsFailed = result.IsFailed,
                IsSuccess = result.IsSuccess,
                Errors = result.Errors.ToCodeMessageDictionary()
            });
        }

        var isNotFoundResult = result.Errors.Any(err => err is CustomFluentError customFluentError && customFluentError.Code.EndsWith("_NOT_FOUND", StringComparison.OrdinalIgnoreCase));
        if (result.IsFailed && isNotFoundResult)
        {
            return controller.StatusCode((int)HttpStatusCode.NotFound, new ApiResponse<TData>
            {
                IsFailed = result.IsFailed,
                IsSuccess = result.IsSuccess,
                Errors = result.Errors.ToCodeMessageDictionary(),
            });
        }

        var badRequestDetails = new ApiResponse<TData>
        {
            IsFailed = result.IsFailed,
            IsSuccess = result.IsSuccess,
            Errors = result.Errors.ToCodeMessageDictionary(),
        };
            
        return controller.BadRequest(badRequestDetails);
    }
    
    // /// <summary>
    // /// Creates an ActionResult from a non-generic Result.
    // /// </summary>
    // public static ActionResult ToActionResult(this ControllerBase controller, Result result)
    // {
    //     if (result.IsSuccess)
    //     {
    //         return controller.NoContent();
    //     }
    //
    //     if (result.IsFailed && result.HasException<Exception>())
    //     {
    //         var problemDetails = CreateProblemDetailsInternal(controller.HttpContext, HttpStatusCode.InternalServerError, result);
    //         return controller.StatusCode((int)HttpStatusCode.InternalServerError, problemDetails);
    //     }
    //
    //     var badRequestDetails = CreateProblemDetailsInternal(controller.HttpContext, HttpStatusCode.BadRequest, result);
    //     return controller.BadRequest(badRequestDetails);
    // }

    // /// <summary>
    // /// Creates ProblemDetailsWithErrors from a FluentResults Result.
    // /// </summary>
    // private static ProblemDetailsWithErrors CreateProblemDetailsInternal(
    //     HttpContext httpContext, HttpStatusCode httpStatusCode, Result result)
    // {
    //     return new ProblemDetailsWithErrors
    //     {
    //         Type = "about:blank",
    //         Title = httpStatusCode.ToString(),
    //         Instance = httpContext.Request.Path,
    //         Status = (int)httpStatusCode,
    //         Errors = result.Errors.ToCodeMessageDictionary()
    //     };
    // }

    /// <summary>
    /// Converts an IEnumerable of IError to a dictionary grouping error messages.
    /// </summary>
    private static Dictionary<string, string> ToCodeMessageDictionary(this IEnumerable<IError> errors)
    {
        return errors
            .Select(error => error is CustomFluentError customFluentError
                ? new { Code = customFluentError.Code, Message = customFluentError.Message }
                : new { Code = error.Message, Message = error.Message })
            .GroupBy(item => item.Code)
            .ToDictionary(group => group.Key, group => group.First().Message);
    }
    
    /// <summary>
    /// Converts an IEnumerable of IReason to a dictionary grouping reason messages.
    /// </summary>
    private static Dictionary<string, string> ToCodeMessageDictionary(this IEnumerable<IReason> reasons)
    {
        return reasons
            .Select(reasons => reasons is CustomFluentError customFluentError
                ? new { Code = customFluentError.Code, Message = customFluentError.Message }
                : new { Code = reasons.Message, Message = reasons.Message })
            .GroupBy(item => item.Code)
            .ToDictionary(group => group.Key, group => group.First().Message);
    }
}



/// <summary>
/// A ProblemDetails extension that includes a dictionary of errors.
/// </summary>
public class ProblemDetailsWithErrors : ProblemDetails
{
    public Dictionary<string, string> Errors { get; set; } = new(StringComparer.Ordinal);
}