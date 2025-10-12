using System;
using System.Collections.Generic;
using System.Linq;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Domain.Common.Pagination;

/// <summary>
/// Utility methods for working with pagination.
/// </summary>
public static class PaginationHelpers
{
    /// <summary>
    /// The default page size for pagination requests.
    /// </summary>
    public const int DefaultPageSize = 10;

    /// <summary>
    /// The maximum allowed page size for pagination requests.
    /// </summary>
    public const int MaxPageSize = 100;

    /// <summary>
    /// Creates a pagination request with validated parameters.
    /// </summary>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="cursor">Optional cursor for cursor-based pagination.</param>
    /// <param name="direction">The pagination direction.</param>
    /// <returns>A validated pagination request.</returns>
    public static PaginationRequest CreateRequest(
        int pageNumber = 1,
        int pageSize = DefaultPageSize,
        string? cursor = null,
        PaginationDirection direction = PaginationDirection.Forward)
    {
        return new PaginationRequest
        {
            PageNumber = Math.Max(1, pageNumber),
            PageSize = Math.Clamp(pageSize, 1, MaxPageSize),
            Cursor = cursor,
            Direction = direction
        };
    }

    /// <summary>
    /// Creates pagination metadata from query parameters.
    /// </summary>
    /// <param name="items">The items in the result.</param>
    /// <param name="totalCount">The total count of items.</param>
    /// <param name="request">The original pagination request.</param>
    /// <returns>Pagination metadata.</returns>
    public static PaginationMetadata CreateMetadata<T>(
        IEnumerable<T> items,
        long totalCount,
        PaginationRequest request) where T : IEntity<Ulid>
    {
        var itemsList = items.ToList();
        var totalPages = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / request.PageSize) : 0;

        return new PaginationMetadata
        {
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            HasNextPage = request.PageNumber < totalPages,
            HasPreviousPage = request.PageNumber > 1,
            CurrentPageSize = itemsList.Count,
            FirstCursor = itemsList.Count > 0 ? itemsList[0].Id.ToString() : null,
            LastCursor = itemsList.Count > 0 ? itemsList[^1].Id.ToString() : null
        };
    }

    /// <summary>
    /// Validates pagination parameters and returns a normalized request.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="cursor">The cursor.</param>
    /// <param name="direction">The direction.</param>
    /// <returns>A validated pagination request.</returns>
    /// <exception cref="ArgumentException">Thrown when parameters are invalid.</exception>
    public static PaginationRequest ValidateAndCreate(
        int? pageNumber = null,
        int? pageSize = null,
        string? cursor = null,
        string? direction = null)
    {
        var normalizedPageNumber = Math.Max(1, pageNumber ?? 1);
        var normalizedPageSize = Math.Clamp(pageSize ?? DefaultPageSize, 1, MaxPageSize);
        
        var normalizedDirection = PaginationDirection.Forward;
        if (!string.IsNullOrWhiteSpace(direction))
        {
            if (!Enum.TryParse<PaginationDirection>(direction, true, out normalizedDirection))
            {
                normalizedDirection = PaginationDirection.Forward;
            }
        }

        var request = new PaginationRequest
        {
            PageNumber = normalizedPageNumber,
            PageSize = normalizedPageSize,
            Cursor = cursor,
            Direction = normalizedDirection
        };

        if (!request.IsValid())
        {
            throw new ArgumentException("Invalid pagination parameters");
        }

        return request;
    }

    /// <summary>
    /// Creates a cursor from an entity ID.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A cursor string.</returns>
    public static string CreateCursor<T>(T entity) where T : IEntity<Ulid>
    {
        ArgumentNullException.ThrowIfNull(entity);
        return entity.Id.ToString();
    }

    /// <summary>
    /// Parses a cursor string to a Ulid.
    /// </summary>
    /// <param name="cursor">The cursor string.</param>
    /// <returns>The parsed Ulid, or null if invalid.</returns>
    public static Ulid? ParseCursor(string? cursor)
    {
        if (string.IsNullOrWhiteSpace(cursor))
        {
            return null;
        }

        return Ulid.TryParse(cursor, out var id) ? id : null;
    }

    /// <summary>
    /// Calculates the skip count for offset-based pagination.
    /// </summary>
    /// <param name="pageNumber">The page number (1-based).</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>The number of items to skip.</returns>
    public static int CalculateSkip(int pageNumber, int pageSize)
    {
        return (Math.Max(1, pageNumber) - 1) * Math.Max(1, pageSize);
    }
}
