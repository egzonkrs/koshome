using Ardalis.Specification;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Countries;

namespace KosHome.Application.Countries.Specifications;

public sealed class CountriesPaginationSpecification : PaginationSpecification<Country>
{
    public CountriesPaginationSpecification(PaginationRequest paginationRequest) 
        : base(paginationRequest)
    {
    }

    protected override void ApplyFilters()
    {
    }

    protected override void ApplyOrdering()
    {
        Query.OrderBy(country => country.CountryName.Value)
             .ThenBy(country => country.Id);
    }
}

