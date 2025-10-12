using System;
using Ardalis.Specification;
using KosHome.Domain.Data.Abstractions;

namespace KosHome.Domain.Common.Pagination;

public abstract class PaginationSpecification<T> : Specification<T>, IPaginationSpecification<T> 
    where T : class, IEntity<Ulid>
{
    public PaginationRequest PaginationRequest { get; }

    public virtual string? CursorProperty => "Id";

    protected PaginationSpecification(PaginationRequest paginationRequest)
    {
        ArgumentNullException.ThrowIfNull(paginationRequest);
        PaginationRequest = paginationRequest;
        
        ApplyFilters();
        ApplyOrdering();
        ApplyPaginationLogic();
    }

    protected abstract void ApplyFilters();
    
    protected abstract void ApplyOrdering();

    private void ApplyPaginationLogic()
    {
        var isCursorBased = PaginationRequest.IsCursorBased;
        if (isCursorBased)
        {
            ApplyCursorPagination();
            return;
        }
        
        ApplyOffsetPagination();
    }

    private void ApplyCursorPagination()
    {
        var canParseCursor = Ulid.TryParse(PaginationRequest.Cursor, out var cursorId);
        if (!canParseCursor)
        {
            Query.Take(PaginationRequest.PageSize + 1);
            return;
        }

        var isForwardDirection = PaginationRequest.Direction == PaginationDirection.Forward;
        if (isForwardDirection)
        {
            Query.Where(entity => entity.Id.CompareTo(cursorId) > 0);
        }
        else
        {
            Query.Where(entity => entity.Id.CompareTo(cursorId) < 0);
        }
        
        Query.Take(PaginationRequest.PageSize + 1);
    }

    private void ApplyOffsetPagination()
    {
        Query.Skip(PaginationRequest.Skip).Take(PaginationRequest.PageSize);
    }
}
