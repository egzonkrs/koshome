using System.Collections.Generic;
using FluentResults;
using KosHome.Api.Extensions.Controller;
using KosHome.Api.Models;
using KosHome.Api.Models.Common;
using KosHome.Domain.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace KosHome.Api.Extensions.Common;

/// <summary>
/// Extension methods for handling pagination responses.
/// </summary>
public static class PaginationExtensions
{
    /// <summary>
    /// Converts a Result containing a PaginatedResult to an ActionResult with proper ApiResponse format.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated result.</typeparam>
    /// <param name="controller">The controller instance.</param>
    /// <param name="result">The result containing the paginated data.</param>
    /// <returns>An ActionResult with the appropriate response.</returns>
    public static IActionResult ToPaginatedActionResult<T>(
        this ControllerBase controller, 
        Result<PaginatedResult<T>> result)
    {
        if (result.IsSuccess)
        {
            var paginatedResponse = new PaginatedResponse<T>(result.Value);
            var apiResponse = new ApiResponse<PaginatedResponse<T>>
            {
                Data = paginatedResponse,
                IsSuccess = true,
                IsFailed = false,
                Reasons = new Dictionary<string, string>(),
                Errors = new Dictionary<string, string>()
            };
            return controller.Ok(apiResponse);
        }

        return controller.ToActionResult(result);
    }
}
