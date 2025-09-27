using System;

namespace KosHome.Domain.Common.Pagination;

/// <summary>
/// Represents a pagination request that supports both offset-based and cursor-based pagination.
/// </summary>
public sealed record PaginationRequest
{
    /// <summary>
    /// The maximum number of items to return per page.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// The page number for offset-based pagination (1-based).
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// The cursor value for cursor-based pagination.
    /// This should be the ID of the last item from the previous page.
    /// </summary>
    public string? Cursor { get; init; }

    /// <summary>
    /// The direction for cursor-based pagination.
    /// </summary>
    public PaginationDirection Direction { get; init; } = PaginationDirection.Forward;

    /// <summary>
    /// Validates the pagination request parameters.
    /// </summary>
    /// <returns>True if the request is valid, otherwise false.</returns>
    public bool IsValid()
    {
        return PageSize > 0 && PageSize <= 100 && PageNumber > 0;
    }

    /// <summary>
    /// Indicates whether this request uses cursor-based pagination.
    /// </summary>
    public bool IsCursorBased => !string.IsNullOrWhiteSpace(Cursor);

    /// <summary>
    /// Calculates the skip count for offset-based pagination.
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Creates a new pagination request for the next page using cursor-based pagination.
    /// </summary>
    /// <param name="cursor">The cursor value for the next page.</param>
    /// <returns>A new pagination request for the next page.</returns>
    public PaginationRequest WithCursor(string cursor) => this with { Cursor = cursor };

    /// <summary>
    /// Creates a new pagination request for the next page using offset-based pagination.
    /// </summary>
    /// <returns>A new pagination request for the next page.</returns>
    public PaginationRequest NextPage() => this with { PageNumber = PageNumber + 1 };

    /// <summary>
    /// Creates a new pagination request for the previous page using offset-based pagination.
    /// </summary>
    /// <returns>A new pagination request for the previous page.</returns>
    public PaginationRequest PreviousPage() => this with { PageNumber = Math.Max(1, PageNumber - 1) };
}

