using System;
using Ardalis.Specification;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Apartments.Specifications;

/// <summary>
/// Specification for filtering apartments by city.
/// </summary>
public sealed class ApartmentByCitySpecification : Specification<Apartment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentByCitySpecification"/> class.
    /// </summary>
    /// <param name="cityId">The city ID to filter by.</param>
    public ApartmentByCitySpecification(Ulid cityId)
    {
        Query.Where(apartment => apartment.CityId == cityId);
    }
}
