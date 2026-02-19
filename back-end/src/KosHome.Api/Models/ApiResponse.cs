using FluentResults;
using System.Collections.Generic;

namespace KosHome.Api.Models;

/// <summary>
/// Represents a standardized API response.
/// </summary>
/// <typeparam name="TData">The type of the value returned in the response.</typeparam>
public record ApiResponse<TData>
{
    /// <summary>
    /// Gets the value returned from the operation.
    /// </summary>
    public TData Data { get; init; }

    /// <summary>
    /// Gets a value indicating whether the operation has failed.
    /// </summary>
    public bool IsFailed { get; init; }

    /// <summary>
    /// Gets a value indicating whether the operation is successful.
    /// </summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Gets the collection of reasons associated with the response.
    /// </summary>
    public Dictionary<string, string> Reasons { get; init; }

    /// <summary>
    /// Gets the collection of errors associated with the response.
    /// </summary>
    public Dictionary<string, string> Errors { get; init; }
}