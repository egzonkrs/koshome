using System;
using Ardalis.Specification;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Application.Cities.Specifications;

/// <summary>
/// Specification for filtering cities by country.
/// </summary>
public sealed class CitiesByCountrySpecification : Specification<City>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CitiesByCountrySpecification"/> class.
    /// </summary>
    /// <param name="countryId">The country ID to filter by.</param>
    public CitiesByCountrySpecification(Ulid countryId)
    {
        Query.Where(city => city.CountryId == countryId)
             .OrderBy(city => city.CityName.Value);
    }
}
