using System;
using Ardalis.Specification;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Application.Cities.Specifications;

/// <summary>
/// Specification for getting cities with enhanced pagination and filtering support.
/// </summary>
public sealed class CitiesPaginationSpecification : PaginationSpecification<City>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CitiesPaginationSpecification"/> class.
    /// </summary>
    /// <param name="paginationRequest">The pagination request.</param>
    /// <param name="countryId">Optional country ID filter.</param>
    /// <param name="searchTerm">Optional search term for city name.</param>
    public CitiesPaginationSpecification(
        PaginationRequest paginationRequest, 
        Ulid? countryId = null, 
        string? searchTerm = null) 
        : base(paginationRequest)
    {
        // Apply country filter if provided
        if (countryId.HasValue)
        {
            Query.Where(city => city.CountryId == countryId.Value);
        }

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            Query.Where(city => city.CityName.Value.Contains(searchTerm.Trim()));
        }
    }

    /// <summary>
    /// Applies custom ordering for cities.
    /// </summary>
    protected override void ApplyOrdering()
    {
        // Order by city name alphabetically, then by ID for stable pagination
        Query.OrderBy(city => city.CityName.Value)
             .ThenBy(city => city.Id);
    }
}
