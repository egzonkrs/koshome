using Ardalis.Specification;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Application.Cities.Specifications;

/// <summary>
/// Specification for getting all cities ordered by name.
/// </summary>
public sealed class AllCitiesSpecification : Specification<City>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AllCitiesSpecification"/> class.
    /// </summary>
    public AllCitiesSpecification()
    {
        Query.OrderBy(city => city.CityName.Value);
    }
}
