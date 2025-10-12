using Ardalis.Specification;
using KosHome.Domain.Entities.Cities;
using KosHome.Domain.ValueObjects.Cities;

namespace KosHome.Application.Cities.Specifications;

/// <summary>
/// Specification for finding a city by name.
/// </summary>
public sealed class CityByNameSpecification : Specification<City>, ISingleResultSpecification<City>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CityByNameSpecification"/> class.
    /// </summary>
    /// <param name="name">The city name to search for.</param>
    public CityByNameSpecification(CityName name)
    {
        Query.Where(city => city.CityName == name);
    }
}
