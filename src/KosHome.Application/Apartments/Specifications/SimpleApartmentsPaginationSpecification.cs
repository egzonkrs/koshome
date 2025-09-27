using System;
using Ardalis.Specification;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Apartments.Specifications;

/// <summary>
/// Simple specification for getting apartments with basic pagination support.
/// </summary>
public sealed class SimpleApartmentsPaginationSpecification : Specification<Apartment>, IPaginationSpecification<Apartment>
{
    /// <inheritdoc />
    public PaginationRequest PaginationRequest { get; }

    /// <inheritdoc />
    public string? CursorProperty => "Id";

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleApartmentsPaginationSpecification"/> class.
    /// </summary>
    /// <param name="paginationRequest">The pagination request.</param>
    public SimpleApartmentsPaginationSpecification(PaginationRequest paginationRequest)
    {
        PaginationRequest = paginationRequest;
        if (paginationRequest.IsCursorBased && Ulid.TryParse(paginationRequest.Cursor, out var cursorId))
        {
            if (paginationRequest.Direction == PaginationDirection.Forward)
            {
                Query.Where(apartment => apartment.Id.CompareTo(cursorId) > 0);
            }
            else
            {
                Query.Where(apartment => apartment.Id.CompareTo(cursorId) < 0);
            }
            
            Query.Take(paginationRequest.PageSize + 1); // Extra item to check for next page
        }
        else
        {
            Query.Skip(paginationRequest.Skip)
                 .Take(paginationRequest.PageSize);
        }

        // Order by creation date, then by ID for stability
        Query.OrderByDescending(apartment => apartment.CreatedAt)
             .ThenByDescending(apartment => apartment.Id);
    }
}
