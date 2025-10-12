using System;

namespace KosHome.Domain.Common.Pagination;

/// <summary>
/// Contains metadata about pagination state and navigation.
/// </summary>
public sealed record PaginationMetadata
{
    /// <summary>
    /// The current page number (1-based).
    /// </summary>
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// The number of items per page.
    /// </summary>
    public int PageSize { get; init; } = 10;

    /// <summary>
    /// The total number of items across all pages.
    /// </summary>
    public long TotalCount { get; init; }

    /// <summary>
    /// The total number of pages.
    /// </summary>
    public int TotalPages => TotalCount > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>
    /// Indicates whether there is a next page available.
    /// </summary>
    public bool HasNextPage { get; init; }

    /// <summary>
    /// Indicates whether there is a previous page available.
    /// </summary>
    public bool HasPreviousPage { get; init; }

    /// <summary>
    /// The cursor for the first item on the current page (for cursor-based pagination).
    /// </summary>
    public string? FirstCursor { get; init; }

    /// <summary>
    /// The cursor for the last item on the current page (for cursor-based pagination).
    /// </summary>
    public string? LastCursor { get; init; }

    /// <summary>
    /// The number of items in the current page.
    /// </summary>
    public int CurrentPageSize { get; init; }

    /// <summary>
    /// Indicates whether this is the first page.
    /// </summary>
    public bool IsFirstPage => PageNumber == 1;

    /// <summary>
    /// Indicates whether this is the last page.
    /// </summary>
    public bool IsLastPage => !HasNextPage;
}

