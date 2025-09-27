using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Domain.Common.Pagination;

/// <summary>
/// Extension methods for IRepositoryBase to support pagination.
/// </summary>
public static class IRepositoryExtensions
{
    /// <summary>
    /// Executes a paginated query using a pagination specification.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="repository">The repository.</param>
    /// <param name="specification">The pagination specification.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated result.</returns>
    public static async Task<PaginatedResult<T>> GetPaginatedAsync<T>(
        this IRepositoryBase<T> repository,
        IPaginationSpecification<T> specification,
        CancellationToken cancellationToken = default)
        where T : class, IEntity<Ulid>
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(specification);

        var paginationRequest = specification.PaginationRequest;

        if (paginationRequest.IsCursorBased)
        {
            return await GetCursorPaginatedResultAsync(repository, specification, cancellationToken);
        }
        else
        {
            return await GetOffsetPaginatedResultAsync(repository, specification, cancellationToken);
        }
    }

    /// <summary>
    /// Executes a simple paginated query without a custom specification.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    /// <param name="repository">The repository.</param>
    /// <param name="paginationRequest">The pagination request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paginated result.</returns>
    public static Task<PaginatedResult<T>> GetPaginatedAsync<T>(
        this IRepositoryBase<T> repository,
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
        where T : class, IEntity<Ulid>
    {
        var specification = new SimplePaginationSpecification<T>(paginationRequest);
        return repository.GetPaginatedAsync(specification, cancellationToken);
    }

    /// <summary>
    /// Gets cursor-based paginated results.
    /// </summary>
    private static async Task<PaginatedResult<T>> GetCursorPaginatedResultAsync<T>(
        IRepositoryBase<T> repository,
        IPaginationSpecification<T> specification,
        CancellationToken cancellationToken)
        where T : class, IEntity<Ulid>
    {
        var paginationRequest = specification.PaginationRequest;
        var items = await repository.ListAsync(specification, cancellationToken);
        
        var hasNextPage = items.Count > paginationRequest.PageSize;
        if (hasNextPage)
        {
            items = items.Take(paginationRequest.PageSize).ToList();
        }

        var hasPreviousPage = !string.IsNullOrWhiteSpace(paginationRequest.Cursor);

        var metadata = new PaginationMetadata
        {
            PageNumber = paginationRequest.PageNumber,
            PageSize = paginationRequest.PageSize,
            TotalCount = -1, // Not available for cursor-based pagination
            HasNextPage = hasNextPage,
            HasPreviousPage = hasPreviousPage,
            CurrentPageSize = items.Count,
            FirstCursor = items.Count > 0 ? items[0].Id.ToString() : null,
            LastCursor = items.Count > 0 ? items[^1].Id.ToString() : null
        };

        return new PaginatedResult<T>(items, metadata);
    }

    /// <summary>
    /// Gets offset-based paginated results.
    /// </summary>
    private static async Task<PaginatedResult<T>> GetOffsetPaginatedResultAsync<T>(
        IRepositoryBase<T> repository,
        IPaginationSpecification<T> specification,
        CancellationToken cancellationToken)
        where T : class, IEntity<Ulid>
    {
        var paginationRequest = specification.PaginationRequest;

        // Get total count using a count specification
        var countSpec = new CountSpecification<T>(specification);
        var totalCount = await repository.CountAsync(countSpec, cancellationToken);

        // Get the actual items
        var items = await repository.ListAsync(specification, cancellationToken);

        var totalPages = totalCount > 0 ? (int)Math.Ceiling((double)totalCount / paginationRequest.PageSize) : 0;
        var hasNextPage = paginationRequest.PageNumber < totalPages;
        var hasPreviousPage = paginationRequest.PageNumber > 1;

        var metadata = new PaginationMetadata
        {
            PageNumber = paginationRequest.PageNumber,
            PageSize = paginationRequest.PageSize,
            TotalCount = totalCount,
            HasNextPage = hasNextPage,
            HasPreviousPage = hasPreviousPage,
            CurrentPageSize = items.Count,
            FirstCursor = items.Count > 0 ? items[0].Id.ToString() : null,
            LastCursor = items.Count > 0 ? items[^1].Id.ToString() : null
        };

        return new PaginatedResult<T>(items, metadata);
    }

    /// <summary>
    /// A simple specification for basic pagination without additional filters.
    /// </summary>
    private sealed class SimplePaginationSpecification<T> : PaginationSpecification<T>
        where T : class, IEntity<Ulid>
    {
        public SimplePaginationSpecification(PaginationRequest paginationRequest) 
            : base(paginationRequest)
        {
        }
    }

    /// <summary>
    /// A specification that derives count logic from another specification.
    /// </summary>
    private sealed class CountSpecification<T> : Specification<T>
        where T : class
    {
        public CountSpecification(Ardalis.Specification.ISpecification<T> sourceSpecification)
        {
            // Copy where conditions but not ordering, paging, or includes
            if (sourceSpecification is Specification<T> spec)
            {
                foreach (var whereExpression in spec.WhereExpressions)
                {
                    Query.Where(whereExpression.Filter);
                }

                foreach (var searchCriteria in spec.SearchCriterias)
                {
                    Query.Search(searchCriteria.Selector, searchCriteria.SearchTerm, searchCriteria.SearchGroup);
                }
            }
        }
    }
}
