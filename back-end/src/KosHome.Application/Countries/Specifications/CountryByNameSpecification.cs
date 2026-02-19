using Ardalis.Specification;
using KosHome.Domain.Entities.Countries;
using KosHome.Domain.ValueObjects.Countries;

namespace KosHome.Application.Countries.Specifications;

/// <summary>
/// Specification for finding a country by name.
/// </summary>
public sealed class CountryByNameSpecification : Specification<Country>, ISingleResultSpecification<Country>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CountryByNameSpecification"/> class.
    /// </summary>
    /// <param name="name">The country name to search for.</param>
    public CountryByNameSpecification(CountryName name)
    {
        Query.Where(country => country.CountryName == name);
    }
}
