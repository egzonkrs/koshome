using System;
using System.Linq.Expressions;
using Ardalis.Specification;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Domain.Common.Pagination;

/// <summary>
/// A base specification that provides pagination functionality for entities with Ulid identifiers.
/// </summary>
/// <typeparam name="T">The entity type that implements IEntity&lt;Ulid&gt;.</typeparam>
public abstract class PaginationSpecification<T> : Specification<T>, IPaginationSpecification<T> 
    where T : class, IEntity<Ulid>
{
    /// <inheritdoc />
    public PaginationRequest PaginationRequest { get; }

    /// <inheritdoc />
    public virtual string? CursorProperty => "Id";

    /// <summary>
    /// Initializes a new instance of the <see cref="PaginationSpecification{T}"/> class.
    /// </summary>
    /// <param name="paginationRequest">The pagination request.</param>
    protected PaginationSpecification(PaginationRequest paginationRequest)
    {
        ArgumentNullException.ThrowIfNull(paginationRequest);

        if (!paginationRequest.IsValid())
        {
            throw new ArgumentException("Invalid pagination request.", nameof(paginationRequest));
        }

        PaginationRequest = paginationRequest;

        ApplyPagination();
    }

    /// <summary>
    /// Applies pagination logic to the specification.
    /// </summary>
    private void ApplyPagination()
    {
        // Apply custom ordering first (if any)
        ApplyOrdering();
        
        if (PaginationRequest.IsCursorBased)
        {
            ApplyCursorPagination();
        }
        else
        {
            ApplyOffsetPagination();
        }
    }

    /// <summary>
    /// Applies custom ordering. Override this method in derived classes to specify custom ordering.
    /// </summary>
    protected virtual void ApplyOrdering()
    {
        // Default ordering by Id for stability
        Query.OrderBy(x => x.Id);
    }

    /// <summary>
    /// Applies cursor-based pagination.
    /// </summary>
    private void ApplyCursorPagination()
    {
        if (Ulid.TryParse(PaginationRequest.Cursor, out var cursorId))
        {
            if (PaginationRequest.Direction == PaginationDirection.Forward)
            {
                Query.Where(x => x.Id.CompareTo(cursorId) > 0);
            }
            else
            {
                Query.Where(x => x.Id.CompareTo(cursorId) < 0);
                // Note: Ordering should be handled by ApplyOrdering method to avoid conflicts
            }
        }

        // Take one extra item to determine if there's a next page
        Query.Take(PaginationRequest.PageSize + 1);
    }

    /// <summary>
    /// Applies offset-based pagination.
    /// </summary>
    private void ApplyOffsetPagination()
    {
        Query.Skip(PaginationRequest.Skip)
             .Take(PaginationRequest.PageSize);
    }

    /// <summary>
    /// Creates an ordering expression for a property.
    /// </summary>
    /// <param name="propertyExpression">The property expression.</param>
    /// <param name="ascending">Whether to order ascending.</param>
    protected void OrderBy(Expression<Func<T, object?>> propertyExpression, bool ascending = true)
    {
        if (ascending)
        {
            Query.OrderBy(propertyExpression);
        }
        else
        {
            Query.OrderByDescending(propertyExpression);
        }
    }

    /// <summary>
    /// Adds a secondary ordering expression for a property.
    /// </summary>
    /// <param name="propertyExpression">The property expression.</param>
    /// <param name="ascending">Whether to order ascending.</param>
    protected void ThenBy(Expression<Func<T, object?>> propertyExpression, bool ascending = true)
    {
        if (ascending)
        {
            ((IOrderedSpecificationBuilder<T>)Query).ThenBy(propertyExpression);
        }
        else
        {
            ((IOrderedSpecificationBuilder<T>)Query).ThenByDescending(propertyExpression);
        }
    }
}
