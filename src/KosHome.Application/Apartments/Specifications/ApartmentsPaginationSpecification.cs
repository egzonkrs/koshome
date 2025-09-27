using System;
using Ardalis.Specification;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Apartments.Specifications;

/// <summary>
/// Specification for getting apartments with advanced pagination and filtering support.
/// </summary>
public sealed class ApartmentsPaginationSpecification : PaginationSpecification<Apartment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentsPaginationSpecification"/> class.
    /// </summary>
    /// <param name="paginationRequest">The pagination request.</param>
    /// <param name="cityId">Optional city ID filter.</param>
    /// <param name="minPrice">Optional minimum price filter.</param>
    /// <param name="maxPrice">Optional maximum price filter.</param>
    /// <param name="searchTerm">Optional search term for title or description.</param>
    public ApartmentsPaginationSpecification(
        PaginationRequest paginationRequest,
        Ulid? cityId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? searchTerm = null) 
        : base(paginationRequest)
    {
        // Apply city filter if provided
        if (cityId.HasValue)
        {
            Query.Where(apartment => apartment.CityId == cityId.Value);
        }

        // Apply minimum price filter if provided
        if (minPrice.HasValue)
        {
            Query.Where(apartment => apartment.Price.Value >= minPrice.Value);
        }

        // Apply maximum price filter if provided
        if (maxPrice.HasValue)
        {
            Query.Where(apartment => apartment.Price.Value <= maxPrice.Value);
        }

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var trimmedSearchTerm = searchTerm.Trim();
            Query.Where(apartment => 
                apartment.Title.Value.Contains(trimmedSearchTerm) ||
                apartment.Description.Value.Contains(trimmedSearchTerm));
        }
    }

    /// <summary>
    /// Applies custom ordering for apartments.
    /// </summary>
    protected override void ApplyOrdering()
    {
        // Order by apartment creation date (newest first), then by ID for stability
        Query.OrderByDescending(apartment => apartment.CreatedAt)
             .ThenBy(apartment => apartment.Id);
    }
}
