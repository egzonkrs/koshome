using System;
using Ardalis.Specification;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Application.Cities.Specifications;

/// <summary>
/// Simple specification for getting cities with basic pagination support.
/// </summary>
public sealed class SimpleCitiesPaginationSpecification : Specification<City>, IPaginationSpecification<City>
{
    /// <inheritdoc />
    public PaginationRequest PaginationRequest { get; }

    /// <inheritdoc />
    public string? CursorProperty => "Id";

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleCitiesPaginationSpecification"/> class.
    /// </summary>
    /// <param name="paginationRequest">The pagination request.</param>
    public SimpleCitiesPaginationSpecification(PaginationRequest paginationRequest)
    {
        PaginationRequest = paginationRequest;
        
        if (paginationRequest.IsCursorBased && Ulid.TryParse(paginationRequest.Cursor, out var cursorId))
        {
            if (paginationRequest.Direction == PaginationDirection.Forward)
            {
                Query.Where(city => city.Id.CompareTo(cursorId) > 0);
            }
            else
            {
                Query.Where(city => city.Id.CompareTo(cursorId) < 0);
            }
            
            Query.Take(paginationRequest.PageSize + 1); // Extra item to check for next page
        }
        else
        {
            Query.Skip(paginationRequest.Skip)
                 .Take(paginationRequest.PageSize);
        }

        // Order by city name alphabetically, then by ID for stability
        Query.OrderBy(city => city.CityName.Value)
             .ThenBy(city => city.Id);
    }
}

