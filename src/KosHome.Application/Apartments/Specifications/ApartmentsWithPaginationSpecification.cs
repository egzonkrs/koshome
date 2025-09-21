using Ardalis.Specification;
using KosHome.Domain.Entities.Apartments;

namespace KosHome.Application.Apartments.Specifications;

/// <summary>
/// Specification for getting apartments with pagination support.
/// </summary>
public sealed class ApartmentsWithPaginationSpecification : Specification<Apartment>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentsWithPaginationSpecification"/> class.
    /// </summary>
    /// <param name="skip">Number of items to skip.</param>
    /// <param name="take">Number of items to take.</param>
    public ApartmentsWithPaginationSpecification(int skip, int take)
    {
        Query.OrderBy(apartment => apartment.CreatedAt)
             .Skip(skip)
             .Take(take);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApartmentsWithPaginationSpecification"/> class.
    /// Gets all apartments without pagination.
    /// </summary>
    public ApartmentsWithPaginationSpecification()
    {
        Query.OrderBy(apartment => apartment.CreatedAt);
    }
}
