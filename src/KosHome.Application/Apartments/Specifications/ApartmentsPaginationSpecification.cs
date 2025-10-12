using System;
using Ardalis.Specification;
using KosHome.Domain.Common.Pagination;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Apartments.Specifications;

public sealed class ApartmentsPaginationSpecification : PaginationSpecification<Apartment>
{
    private readonly Ulid? _cityId;
    private readonly decimal? _minPrice;
    private readonly decimal? _maxPrice;

    public ApartmentsPaginationSpecification(
        PaginationRequest paginationRequest,
        Ulid? cityId = null,
        decimal? minPrice = null,
        decimal? maxPrice = null) : base(paginationRequest)
    {
        _cityId = cityId;
        _minPrice = minPrice;
        _maxPrice = maxPrice;
    }

    protected override void ApplyFilters()
    {
        var hasCityFilter = _cityId.HasValue;
        if (hasCityFilter)
        {
            Query.Where(apartment => apartment.CityId == _cityId.Value);
        }

        var hasMinPriceFilter = _minPrice.HasValue;
        if (hasMinPriceFilter)
        {
            Query.Where(apartment => apartment.Price.Value >= _minPrice.Value);
        }

        var hasMaxPriceFilter = _maxPrice.HasValue;
        if (hasMaxPriceFilter)
        {
            Query.Where(apartment => apartment.Price.Value <= _maxPrice.Value);
        }
    }

    protected override void ApplyOrdering()
    {
        Query.OrderByDescending(apartment => apartment.CreatedAt)
             .ThenByDescending(apartment => apartment.Id);
    }
}

