using System;
using Ardalis.Specification;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Cities;

namespace KosHome.Application.Cities.Specifications;

public sealed class CitiesPaginationSpecification : PaginationSpecification<City>
{
    private readonly Ulid? _countryId;

    public CitiesPaginationSpecification(PaginationRequest paginationRequest, Ulid? countryId = null) : base(paginationRequest)
    {
        _countryId = countryId;
    }

    protected override void ApplyFilters()
    {
        var hasCountryFilter = _countryId.HasValue;
        if (hasCountryFilter)
        {
            Query.Where(city => city.CountryId == _countryId.Value);
        }
    }

    protected override void ApplyOrdering()
    {
        Query.OrderBy(city => city.CityName.Value)
             .ThenBy(city => city.Id);
    }
}

