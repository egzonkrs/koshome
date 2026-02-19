using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Specification;
using FluentResults;
using KosHome.Application.Abstractions.Pagination;
using KosHome.Domain.Common;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Application.Common.Services;

public sealed class PaginationService : IPaginationService
{
    public async Task<Result<PaginatedResult<TEntity>>> GetPaginatedAsync<TEntity>(
        IRepositoryBase<TEntity> repository,
        IPaginationSpecification<TEntity> specification,
        CancellationToken cancellationToken = default) where TEntity : class, IEntity<Ulid>
    {
        ArgumentNullException.ThrowIfNull(repository);
        ArgumentNullException.ThrowIfNull(specification);

        var paginationRequest = specification.PaginationRequest;

        var validationResult = ValidatePaginationRequest(paginationRequest);
        var isInvalidRequest = validationResult.IsFailed;
        if (isInvalidRequest)
        {
            return Result.Fail(validationResult.Errors);
        }

        var isCursorBased = paginationRequest.IsCursorBased;
        if (isCursorBased)
        {
            return await GetCursorPaginatedAsync(repository, specification, cancellationToken);
        }

        return await GetOffsetPaginatedAsync(repository, specification, cancellationToken);
    }

    private static Result ValidatePaginationRequest(PaginationRequest request)
    {
        var isValidPageSize = request.PageSize is > 0 and <= 100;
        if (isValidPageSize is false)
        {
            return Result.Fail(PaginationErrors.InvalidPageSize());
        }

        var isValidPageNumber = request.PageNumber > 0;
        if (isValidPageNumber is false)
        {
            return Result.Fail(PaginationErrors.InvalidPageNumber());
        }

        return Result.Ok();
    }

    private static async Task<Result<PaginatedResult<T>>> GetCursorPaginatedAsync<T>(
        IRepositoryBase<T> repository,
        IPaginationSpecification<T> specification,
        CancellationToken cancellationToken)
        where T : class, IEntity<Ulid>
    {
        var paginationRequest = specification.PaginationRequest;
        var items = await repository.ListAsync(specification, cancellationToken);
        
        var hasMoreItems = items.Count > paginationRequest.PageSize;
        if (hasMoreItems)
        {
            items = items.Take(paginationRequest.PageSize).ToList();
        }

        var hasPreviousCursor = !string.IsNullOrWhiteSpace(paginationRequest.Cursor);
        var hasItems = items.Count > 0;
        var firstCursor = hasItems ? items.First().Id.ToString() : null;
        var lastCursor = hasItems ? items.Last().Id.ToString() : null;

        var metadata = new PaginationMetadata
        {
            PageNumber = paginationRequest.PageNumber,
            PageSize = paginationRequest.PageSize,
            TotalCount = -1,
            HasNextPage = hasMoreItems,
            HasPreviousPage = hasPreviousCursor,
            CurrentPageSize = items.Count,
            FirstCursor = firstCursor,
            LastCursor = lastCursor
        };

        return Result.Ok(new PaginatedResult<T>(items, metadata));
    }

    private static async Task<Result<PaginatedResult<T>>> GetOffsetPaginatedAsync<T>(
        IRepositoryBase<T> repository,
        IPaginationSpecification<T> specification,
        CancellationToken cancellationToken)
        where T : class, IEntity<Ulid>
    {
        var paginationRequest = specification.PaginationRequest;

        var countSpec = new CountOnlySpecification<T>(specification);
        var totalCount = await repository.CountAsync(countSpec, cancellationToken);

        var items = await repository.ListAsync(specification, cancellationToken);

        var totalPages = totalCount > 0 
            ? (int)Math.Ceiling((double)totalCount / paginationRequest.PageSize) 
            : 0;
        
        var hasNextPage = paginationRequest.PageNumber < totalPages;
        var hasPreviousPage = paginationRequest.PageNumber > 1;
        var hasItems = items.Count > 0;
        var firstCursor = hasItems ? items.First().Id.ToString() : null;
        var lastCursor = hasItems ? items.Last().Id.ToString() : null;

        var metadata = new PaginationMetadata
        {
            PageNumber = paginationRequest.PageNumber,
            PageSize = paginationRequest.PageSize,
            TotalCount = totalCount,
            HasNextPage = hasNextPage,
            HasPreviousPage = hasPreviousPage,
            CurrentPageSize = items.Count,
            FirstCursor = firstCursor,
            LastCursor = lastCursor
        };

        return Result.Ok(new PaginatedResult<T>(items, metadata));
    }

    private sealed class CountOnlySpecification<T> : Specification<T> where T : class
    {
        public CountOnlySpecification(Ardalis.Specification.ISpecification<T> sourceSpecification)
        {
            var isArdalisSpecification = sourceSpecification is Specification<T>;
            if (isArdalisSpecification is false)
            {
                return;
            }

            var ardalisSpec = (Specification<T>)sourceSpecification;
            
            foreach (var whereExpression in ardalisSpec.WhereExpressions)
            {
                Query.Where(whereExpression.Filter);
            }
        }
    }
}

