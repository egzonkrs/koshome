using System.ComponentModel.DataAnnotations;
using KosHome.Domain.Common.Pagination;

namespace KosHome.Api.Models.Common;

/// <summary>
/// API model for pagination requests.
/// </summary>
public sealed record PaginationRequest
{
    /// <summary>
    /// The page number (1-based).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int PageNumber { get; init; } = 1;

    /// <summary>
    /// The number of items per page.
    /// </summary>
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; init; } = 2;

    /// <summary>
    /// The cursor for cursor-based pagination.
    /// </summary>
    public string? Cursor { get; init; }

    /// <summary>
    /// The direction for cursor-based pagination.
    /// </summary>
    public string? Direction { get; init; }

    /// <summary>
    /// Converts to domain pagination request.
    /// </summary>
    /// <returns>A domain pagination request.</returns>
    public Domain.Common.Pagination.PaginationRequest ToDomain()
    {
        return PaginationHelpers.ValidateAndCreate(
            PageNumber,
            PageSize,
            Cursor,
            Direction);
    }
}

