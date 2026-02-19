using Ardalis.Specification;
using KosHome.Domain.Entities.Countries;

namespace KosHome.Application.Countries.Specifications;

/// <summary>
/// Specification for getting all countries ordered by name.
/// </summary>
public sealed class AllCountriesSpecification : Specification<Country>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AllCountriesSpecification"/> class.
    /// </summary>
    public AllCountriesSpecification()
    {
        Query.OrderBy(country => country.CountryName.Value);
    }
}
