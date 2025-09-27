using System.Collections.Generic;
using KosHome.Domain.Common.Pagination;

namespace KosHome.Api.Models.Common;

/// <summary>
/// API response model for paginated results.
/// </summary>
/// <typeparam name="T">The type of items in the response.</typeparam>
public sealed record PaginatedResponse<T>
{
    /// <summary>
    /// The items in the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; } = [];

    /// <summary>
    /// Pagination metadata.
    /// </summary>
    public PaginationMetadata Pagination { get; init; } = new();

    /// <summary>
    /// Creates a paginated response from a domain paginated result.
    /// </summary>
    /// <param name="result">The domain paginated result.</param>
    public PaginatedResponse(PaginatedResult<T> result)
    {
        Items = result.Items;
        Pagination = result.Metadata;
    }

    /// <summary>
    /// Creates an empty paginated response.
    /// </summary>
    /// <param name="paginationRequest">The original pagination request.</param>
    /// <returns>An empty paginated response.</returns>
    public static PaginatedResponse<T> Empty(Domain.Common.Pagination.PaginationRequest paginationRequest)
    {
        var emptyResult = PaginatedResult<T>.Empty(paginationRequest);
        return new PaginatedResponse<T>(emptyResult);
    }
}

