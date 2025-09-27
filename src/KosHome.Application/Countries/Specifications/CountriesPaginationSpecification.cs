using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Countries;

namespace KosHome.Application.Countries.Specifications;

/// <summary>
/// Specification for getting countries with enhanced pagination support.
/// </summary>
public sealed class CountriesPaginationSpecification : PaginationSpecification<Country>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CountriesPaginationSpecification"/> class.
    /// </summary>
    /// <param name="paginationRequest">The pagination request.</param>
    public CountriesPaginationSpecification(PaginationRequest paginationRequest) 
        : base(paginationRequest)
    {
        // Order by country name alphabetically, then by ID for stable pagination
        OrderBy(country => (object)country.CountryName.Value);
        ThenBy(country => (object)country.Id);
    }
}
