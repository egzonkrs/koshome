using Ardalis.Specification;

namespace KosHome.Domain.Common.Pagination;

/// <summary>
/// Interface for specifications that support pagination.
/// </summary>
/// <typeparam name="T">The entity type.</typeparam>
public interface IPaginationSpecification<T> : ISpecification<T>
{
    /// <summary>
    /// The pagination request.
    /// </summary>
    PaginationRequest PaginationRequest { get; }

    /// <summary>
    /// The property name used for cursor-based pagination.
    /// </summary>
    string? CursorProperty { get; }
}

