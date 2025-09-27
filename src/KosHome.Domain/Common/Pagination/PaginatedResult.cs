using System;
using System.Collections.Generic;
using System.Linq;

namespace KosHome.Domain.Common.Pagination;

/// <summary>
/// Represents a paginated result with metadata about the pagination state.
/// </summary>
/// <typeparam name="T">The type of items in the result.</typeparam>
public sealed record PaginatedResult<T>
{
    /// <summary>
    /// The items in the current page.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; } = [];

    /// <summary>
    /// The pagination metadata.
    /// </summary>
    public PaginationMetadata Metadata { get; init; } = new();

    /// <summary>
    /// Creates a new paginated result.
    /// </summary>
    /// <param name="items">The items in the current page.</param>
    /// <param name="metadata">The pagination metadata.</param>
    public PaginatedResult(IEnumerable<T> items, PaginationMetadata metadata)
    {
        Items = items.ToList().AsReadOnly();
        Metadata = metadata;
    }

    /// <summary>
    /// Creates an empty paginated result.
    /// </summary>
    /// <param name="request">The original pagination request.</param>
    /// <returns>An empty paginated result.</returns>
    public static PaginatedResult<T> Empty(PaginationRequest request) => new(
        [],
        new PaginationMetadata
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = 0,
            HasNextPage = false,
            HasPreviousPage = false
        });

    /// <summary>
    /// Maps the items to a different type while preserving pagination metadata.
    /// </summary>
    /// <typeparam name="TResult">The target type.</typeparam>
    /// <param name="mapper">The mapping function.</param>
    /// <returns>A paginated result with mapped items.</returns>
    public PaginatedResult<TResult> Map<TResult>(Func<T, TResult> mapper) => new(
        Items.Select(mapper),
        Metadata);
}

