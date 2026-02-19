using Ardalis.Specification;
using KosHome.Domain.Entities.Apartments;
using KosHome.Domain.ValueObjects.Apartments;

namespace KosHome.Application.Apartments.Specifications;

/// <summary>
/// Specification for finding an apartment by title.
/// </summary>
public sealed class ApartmentByTitleSpecification : Specification<Apartment>, ISingleResultSpecification<Apartment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentByTitleSpecification"/> class.
    /// </summary>
    /// <param name="title">The title to search for.</param>
    public ApartmentByTitleSpecification(Title title)
    {
        Query.Where(apartment => apartment.Title == title);
    }
}
